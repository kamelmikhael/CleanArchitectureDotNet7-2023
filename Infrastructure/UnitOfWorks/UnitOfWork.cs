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

namespace Infrastructure.UnitOfWorks;

public class UnitOfWork : IUnitOfWork, IDisposable
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
        //OnBeforeSaveChanges();

        SetAuditProperties();

        return await _context.SaveChangesAsync(cancellationToken);
    }

    private void OnBeforeSaveChanges()
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
            _context.AuditLogs.Add(auditEntry.ToAudit());
        }
    }

    private void SetAuditProperties()
    {
        var utcNow = DateTime.UtcNow;
        string userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        Guid? currentLoggedUserId = userId is null ? null : Guid.Parse(userId);

        HandleCreationAuditedEntities(utcNow, currentLoggedUserId);
        HandleModificationAuditedEntities(utcNow, currentLoggedUserId);
        HandleDeletionAuditedEntities(utcNow, currentLoggedUserId);
    }

    private void HandleCreationAuditedEntities(DateTime utcNow, Guid? currentLoggedUserId)
    {
        _context.ChangeTracker.DetectChanges();
        IEnumerable<EntityEntry> entities = _context.ChangeTracker
                .Entries()
                .Where(t => t.Entity is ICreationAudited && t.State == EntityState.Added);

        foreach (EntityEntry entry in entities)
        {
            UpdateCreatedEntry(utcNow, entry, currentLoggedUserId);

            UpdateModifiedEntry(utcNow, entry, currentLoggedUserId);
        }
    }

    private void HandleModificationAuditedEntities(DateTime utcNow, Guid? currentLoggedUserId)
    {
        _context.ChangeTracker.DetectChanges();
        IEnumerable<EntityEntry> entities = _context.ChangeTracker
                .Entries()
                .Where(t => t.Entity is IModificationAudited && t.State == EntityState.Modified);

        foreach (EntityEntry entry in entities)
        {
            UpdateModifiedEntry(utcNow, entry, currentLoggedUserId);
        }
    }

    private void HandleDeletionAuditedEntities(DateTime utcNow, Guid? currentLoggedUserId)
    {
        _context.ChangeTracker.DetectChanges();
        IEnumerable<EntityEntry> entities = _context.ChangeTracker
                .Entries()
                .Where(t => t.Entity is IDeletionAudited && t.State == EntityState.Deleted);

        foreach (EntityEntry entry in entities)
        {
            UpdateModifiedEntry(utcNow, entry, currentLoggedUserId);
            UpdateDeletedEntry(utcNow, entry, currentLoggedUserId);

            entry.State = EntityState.Modified;
        }
    }

    private static void UpdateCreatedEntry(DateTime utcNow, EntityEntry entry, Guid? currentLoggedUserId)
    {
        ICreationAudited createdAuditEntity = (ICreationAudited)entry.Entity;
        if (createdAuditEntity is not null)
        {
            createdAuditEntity.CreationTime = utcNow;
            createdAuditEntity.CreatorUserId = currentLoggedUserId;
        }
    }

    private static void UpdateDeletedEntry(DateTime utcNow, EntityEntry entry, Guid? currentLoggedUserId)
    {
        IDeletionAudited deleteAuditEntity = (IDeletionAudited)entry.Entity;
        if (deleteAuditEntity is not null)
        {
            deleteAuditEntity.DeletionTime = utcNow;
            deleteAuditEntity.IsDeleted = true;
            deleteAuditEntity.DeleterUserId = currentLoggedUserId;
        }
    }

    private static void UpdateModifiedEntry(DateTime utcNow, EntityEntry entry, Guid? currentLoggedUserId)
    {
        IModificationAudited modifiedAuditEntity = (IModificationAudited)entry.Entity;
        if (modifiedAuditEntity is not null)
        {
            modifiedAuditEntity.LastModificationTime = utcNow;
            modifiedAuditEntity.LastModifierUserId = currentLoggedUserId;
        }
    }

    public void Dispose() => _context.Dispose();
}
