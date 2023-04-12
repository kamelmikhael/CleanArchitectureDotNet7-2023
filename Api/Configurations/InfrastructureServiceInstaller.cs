using Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Api.Configurations;

public class InfrastructureServiceInstaller : IServiceInstaller
{
    public void Install(IServiceCollection services, IConfiguration configuration)
    {
        services.Scan(selector 
            => selector
                .FromAssemblies(Infrastructure.AssemblyReference.Assembly)
                .AddClasses(false)
                .AsImplementedInterfaces()
                .WithScopedLifetime());

        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection"),
                b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));
    }
}
