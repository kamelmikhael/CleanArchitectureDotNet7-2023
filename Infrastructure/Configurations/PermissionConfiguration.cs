using Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Authentications;

namespace Infrastructure.Configurations;

public partial class PermissionConfiguration : IEntityTypeConfiguration<Permission>
{
    public void Configure(EntityTypeBuilder<Permission> entity)
    {
        entity.ToTable("Permissions", "Auth");

        entity.HasKey(p => p.Id);

        entity.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(200);

        var permissions = Enum.GetValues<PermissionEnum>()
            .Select(x => new Permission
            {
                Id = (int)x,
                Name = x.ToString()
            });

        entity.HasData(permissions);

        OnConfigurePartial(entity);
    }

    partial void OnConfigurePartial(EntityTypeBuilder<Permission> entity);
}
