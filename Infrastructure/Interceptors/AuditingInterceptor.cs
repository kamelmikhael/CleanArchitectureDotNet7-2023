using Domain.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Infrastructure.Interceptors;

public sealed class AuditingInterceptor
    : SaveChangesInterceptor
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AuditingInterceptor(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        var dbContext = eventData.Context;

        if (dbContext is null)
        {
            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        var utcNow = DateTime.UtcNow;
        string? userId = null; // _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        Guid? currentLoggedUserId = userId is null ? null : Guid.Parse(userId);

        var creationAuditedEntries = dbContext.ChangeTracker
            .Entries<ICreationAudited>()
            .Where(e => e.State == EntityState.Added)
            .Select(entry =>
            {
                entry.Property(p => p.CreationTime).CurrentValue = utcNow;
                entry.Property(p => p.CreatorUserId).CurrentValue = currentLoggedUserId;

                return entry;
            })
            .ToList();

        var modificationAuditedEntries = dbContext.ChangeTracker
            .Entries<IModificationAudited>()
            .Where(e => e.State == EntityState.Modified)
            .Select(entry =>
            {
                entry.Property(p => p.LastModificationTime).CurrentValue = utcNow;
                entry.Property(p => p.LastModifierUserId).CurrentValue = currentLoggedUserId;

                return entry;
            })
            .ToList();

        var deletionAuditedEntries = dbContext.ChangeTracker
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

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }
}
