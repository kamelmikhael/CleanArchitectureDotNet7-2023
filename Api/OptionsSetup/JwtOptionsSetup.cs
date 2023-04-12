using Infrastructure.Authentications;
using Microsoft.Extensions.Options;

namespace Api.OptionsSetup;

public class JwtOptionsSetup : IConfigureOptions<JwtOptions>
{
    private readonly IConfiguration _configuration;

    public JwtOptionsSetup(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void Configure(JwtOptions options)
    {
        _configuration.GetSection(nameof(JwtOptions)).Bind(options);
    }
}
