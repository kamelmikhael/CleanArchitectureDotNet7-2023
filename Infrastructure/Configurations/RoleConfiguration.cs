using Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Configurations;

public partial class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> entity)
    {
        entity.ToTable("Roles", "Auth");

        entity.HasKey(x => x.Id);

        entity.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(200);

        entity.HasMany(x => x.Permissions)
            .WithMany()
            .UsingEntity<RolePermission>();

        entity.HasMany(x => x.Users)
            .WithMany()
            .UsingEntity<UserRole>();

        entity.HasData(Role.GetValues());

        OnConfigurePartial(entity);
    }

    partial void OnConfigurePartial(EntityTypeBuilder<Role> entity);
}
