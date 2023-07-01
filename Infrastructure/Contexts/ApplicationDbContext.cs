using Domain.Entities;
using Domain.UnitOfWorks;
using Infrastructure.Outbox;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Infrastructure.Contexts;

public class ApplicationDbContext : DbContext
{
    #region Constructors
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }
    #endregion

    #region Methods
    protected override void OnModelCreating(ModelBuilder modelBuilder) =>
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    #endregion
}
