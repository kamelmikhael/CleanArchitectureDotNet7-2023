using Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Authentications;

namespace Infrastructure.Configurations;

public partial class RolePermissionConfiguration : IEntityTypeConfiguration<RolePermission>
{
    public void Configure(EntityTypeBuilder<RolePermission> entity)
    {
        entity.ToTable("RolePermissions", "Auth");

        entity.HasKey(x => new { x.RoleId, x.PermissionId });

        entity.HasData(
            Create(Role.Registered, PermissionEnum.BookAll),
            Create(Role.Registered, PermissionEnum.BookList),
            Create(Role.Registered, PermissionEnum.BookDetail),
            Create(Role.Registered, PermissionEnum.BookCreate),
            Create(Role.Registered, PermissionEnum.BookUpdate),
            Create(Role.Registered, PermissionEnum.BookDelete));

        OnConfigurePartial(entity);
    }

    partial void OnConfigurePartial(EntityTypeBuilder<RolePermission> entity);

    private static RolePermission Create(Role role, PermissionEnum permission)
    {
        return new RolePermission
        {
            RoleId = role.Id,
            PermissionId = (int)permission,
        };
    }
}
public partial class UserRoleConfiguration : IEntityTypeConfiguration<UserRole>
{
    public void Configure(EntityTypeBuilder<UserRole> entity)
    {
        entity.ToTable("UserRoles", "Auth");

        entity.HasKey(x => new { x.RoleId, x.UserId });

        entity.HasData(
            Create(Role.Registered, User.Admin));

        OnConfigurePartial(entity);
    }

    partial void OnConfigurePartial(EntityTypeBuilder<UserRole> entity);

    private static UserRole Create(Role role, User user)
    {
        return new UserRole
        {
            RoleId = role.Id,
            UserId = user.Id,
        };
    }
}