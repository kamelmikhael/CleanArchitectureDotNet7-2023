﻿using Domain.Entities;
using Domain.UnitOfWorks;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Infrastructure.Contexts;

public class ApplicationDbContext : DbContext
{
    #region Constructors
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }
    #endregion

    #region DbSets
    public virtual DbSet<AuditLog> AuditLogs { get; set; }
    #endregion

    #region Methods
    protected override void OnModelCreating(ModelBuilder modelBuilder) =>
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    #endregion
}
