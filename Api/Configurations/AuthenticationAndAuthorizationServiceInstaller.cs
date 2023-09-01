using Api.OptionsSetup;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace Api.Configurations;

public class AuthenticationAndAuthorizationServiceInstaller : IServiceInstaller
{
    public void Install(IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options => options.TokenValidationParameters = new()
            {
                ValidateIssuer = true,
                ValidIssuer = "",

            });

        services.ConfigureOptions<JwtOptionsSetup>();
        services.ConfigureOptions<JwtBearerOptionsSetup>();

        services.AddAuthorization();
        //services.AddSingleton<IAuthorizationHandler, PermissionAuthorizationHandler>();
    }
}
