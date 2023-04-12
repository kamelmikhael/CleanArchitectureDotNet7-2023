using Domain.Repositories;
using Infrastructure.Repositories.Books;

namespace Api.Configurations;

public class CachingServiceInstaller : IServiceInstaller
{
    public void Install(IServiceCollection services, IConfiguration configuration)
    {
        #region In-memory Cache
        services.AddMemoryCache();
        services.AddScoped<BookRepository>();
        services.AddScoped<IBookRepository, CachedBookRepository>();
        #endregion

        #region Distributed Cache using Redis
        services.AddStackExchangeRedisCache(options =>
        {
            string connection = configuration.GetConnectionString("Redis")!;

            options.Configuration = connection;
        });
        #endregion
    }
}

