using Api.OptionsSetup;
using Infrastructure.Authentications;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

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
        //services.AddSingleton<IAuthorizationPolicyProvider, PermissionAuthorizationPolicyProvider>();
        services.AddScoped<IPermissionService, PermissionService>();
    }
}
