using Api.Options;
using Microsoft.Extensions.Options;

namespace Api.OptionsSetup;

public class DatabaseOptionsSetup : IConfigureOptions<DatabaseOptions>
{
    private readonly IConfiguration _configuration;

    public DatabaseOptionsSetup(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void Configure(DatabaseOptions options)
    {
        var connectionString = _configuration.GetConnectionString("DefaultConnection");

        options.ConnectionString = connectionString;

        _configuration.GetSection(nameof(DatabaseOptions)).Bind(options);
    }
}
