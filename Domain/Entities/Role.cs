using Domain.Enums;

namespace Domain.Entities;

public sealed class Role : Enumeration<Role>
{
    public static readonly Role Registered = new(1, nameof(Registered));

    public Role(int id, string name) : base(id, name)
    {
    }

    public ICollection<Permission> Permissions { get; set; }

    public ICollection<User> Users { get; set; }
}
