using Domain.Common;

namespace Domain.Entities;

public sealed class User : Entity<long>
{
    public string Name { get; set; }
    public string Email { get; set; }
}
