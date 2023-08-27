using Domain.Abstractions;
using Domain.UnitOfWorks;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Contexts;
using System.Security.Claims;
using Infrastructure.Auditing;
using Domain.Entities;
using Domain.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Polly;
using Infrastructure.Outbox;
using Newtonsoft.Json;

namespace Infrastructure.UnitOfWorks;

internal sealed class UnitOfWork : IUnitOfWork, IDisposable
{
    private readonly ApplicationDbContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UnitOfWork(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        OnBeforeSaveChanges();

        return await _context.SaveChangesAsync(cancellationToken);
    }

    private void OnBeforeSaveChanges()
    {
        UpdateAuditLogTable();

        ConvertDomainEventsToOutboxMessages();

        SetAuditProperties();
    }

    private void ConvertDomainEventsToOutboxMessages()
    {
        var outboxMessages = _context.ChangeTracker
            .Entries<IAggregateRoot>()
            .Select(x => x.Entity)
            .SelectMany(aggregateRoot =>
            {
                var domainEvents = aggregateRoot.GetDomainEvents();
                aggregateRoot.ClearDomainEvents();
                return domainEvents;
            })
            .Select(domainEvent => new OutboxMessage
            {
                Id = Guid.NewGuid(),
                OccurredOnUtc = DateTime.UtcNow,
                Type = domainEvent.GetType().Name,
                Content = JsonConvert.SerializeObject(
                    domainEvent,
                    new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.All,
                    }),
            })
            .ToList();

        _context.Set<OutboxMessage>().AddRange(outboxMessages);
    }

    private void UpdateAuditLogTable()
    {
        _context.ChangeTracker.DetectChanges();

        var auditEntries = new List<AuditEntry>();

        foreach (var entry in _context.ChangeTracker.Entries())
        {
            if (entry.Entity is AuditLog
                || entry.State == EntityState.Detached
                || entry.State == EntityState.Unchanged)
                continue;

            var auditEntry = new AuditEntry
            {
                TableName = entry.Entity.GetType().Name
            };

            auditEntries.Add(auditEntry);

            foreach (var property in entry.Properties)
            {
                string propertyName = property.Metadata.Name;

                if (property.Metadata.IsPrimaryKey())
                {
                    auditEntry.KeyValues[propertyName] = property.CurrentValue;
                    continue;
                }

                switch (entry.State)
                {
                    case EntityState.Added:
                        auditEntry.AuditType = AuditType.Create;
                        auditEntry.NewValues[propertyName] = property.CurrentValue;
                        break;
                    case EntityState.Deleted:
                        auditEntry.AuditType = AuditType.Delete;
                        auditEntry.OldValues[propertyName] = property.OriginalValue;
                        break;
                    case EntityState.Modified:
                        if (property.IsModified)
                        {
                            auditEntry.ChangedColumns.Add(propertyName);
                            auditEntry.AuditType = AuditType.Update;
                            auditEntry.OldValues[propertyName] = property.OriginalValue;
                            auditEntry.NewValues[propertyName] = property.CurrentValue;
                        }
                        break;
                }
            }
        }

        foreach (var auditEntry in auditEntries)
        {
            _context.Set<AuditLog>().Add(auditEntry.ToAudit());
        }
    }

    private void SetAuditProperties()
    {
        var utcNow = DateTime.UtcNow;
        string? userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        Guid? currentLoggedUserId = userId is null ? null : Guid.Parse(userId);

        SetCreationAuditedEntities(utcNow, currentLoggedUserId);
        SetModificationAuditedEntities(utcNow, currentLoggedUserId);
        SetDeletionAuditedEntities(utcNow, currentLoggedUserId);
    }

    private void SetCreationAuditedEntities(DateTime utcNow, Guid? currentLoggedUserId)
    {
        _ = _context.ChangeTracker
            .Entries<ICreationAudited>()
            .Where(e => e.State == EntityState.Added)
            .Select(entry =>
            {
                entry.Property(p => p.CreationTime).CurrentValue = utcNow;
                entry.Property(p => p.CreatorUserId).CurrentValue = currentLoggedUserId;

                return entry;
            })
            .ToList();
    }

    private void SetModificationAuditedEntities(DateTime utcNow, Guid? currentLoggedUserId)
    {
        _ = _context.ChangeTracker
            .Entries<IModificationAudited>()
            .Where(e => e.State == EntityState.Modified)
            .Select(entry =>
            {
                entry.Property(p => p.LastModificationTime).CurrentValue = utcNow;
                entry.Property(p => p.LastModifierUserId).CurrentValue = currentLoggedUserId;

                return entry;
            })
            .ToList();
    }

    private void SetDeletionAuditedEntities(DateTime utcNow, Guid? currentLoggedUserId)
    {
        _ = _context.ChangeTracker
            .Entries<IDeletionAudited>()
            .Where(e => e.State == EntityState.Deleted)
            .Select(entry =>
            {
                entry.Property(p => p.DeletionTime).CurrentValue = utcNow;
                entry.Property(p => p.DeleterUserId).CurrentValue = currentLoggedUserId;
                entry.Property(p => p.IsDeleted).CurrentValue = true;

                entry.State = EntityState.Modified;

                return entry;
            })
            .ToList();
    }

    public void Dispose() => _context.Dispose();
}
