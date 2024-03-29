﻿using Domain.Common;

namespace Domain.Entities;

public sealed class User : Entity<long>
{
    public string Name { get; set; }

    public string Email { get; set; }

    public ICollection<Role> Roles { get; set; }

    public static readonly User Admin = new User
    {
        Id = 1,
        Name = "admin",
        Email = "admin@test.com",
    };
}
