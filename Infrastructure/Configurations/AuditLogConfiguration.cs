using Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Configurations;

public partial class AuditLogConfiguration : IEntityTypeConfiguration<AuditLog>
{
    public void Configure(EntityTypeBuilder<AuditLog> entity)
    {
        entity.ToTable("AuditLogs", "Audit");

        entity.Property(x => x.Type)
            .HasMaxLength(200);

        entity.Property(x => x.TableName)
            .HasMaxLength(200);

        entity.Property(x => x.PrimaryKey)
            .HasMaxLength(200);

        entity.HasQueryFilter(x => x.IsDeleted == false);

        OnConfigurePartial(entity);
    }

    partial void OnConfigurePartial(EntityTypeBuilder<AuditLog> entity);
}
