namespace Api.Configurations;

public class AuthenticationAndAuthorizationServiceInstaller : IServiceInstaller
{
    public void Install(IServiceCollection services, IConfiguration configuration)
    {
        //services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        //    .AddJwtBearer();
        //services.ConfigureOptions<JwtOptionsSetup>();
        //services.ConfigureOptions<JwtBearerOptionsSetup>();

        //services.AddAuthorization();
        //services.AddSingleton<IAuthorizationHandler, PermissionAuthorizationHandler>();
    }
}
