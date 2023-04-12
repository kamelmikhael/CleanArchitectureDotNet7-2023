using System.Reflection;

namespace Api.Configurations;

public static class DependencyInjection
{
    public static IServiceCollection InstallServices(
        this IServiceCollection services,
        IConfiguration configuration,
        params Assembly[] assemblies)
    {
        assemblies
            .SelectMany(a => a.DefinedTypes)
            .Where(IsAssignableToType<IServiceInstaller>)
            .Select(Activator.CreateInstance)
            .Cast<IServiceInstaller>()
            .Select(serviceInstaller =>
            {
                serviceInstaller.Install(services, configuration);
                return serviceInstaller;
            })
            .ToList();

        return services;

        static bool IsAssignableToType<T>(TypeInfo typeInfo) =>
            typeof(T).IsAssignableFrom(typeInfo) &&
            !typeInfo.IsInterface && 
            !typeInfo.IsAbstract;
    }
}