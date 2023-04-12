using Api.Middlewares;
using Microsoft.AspNetCore.Mvc;

namespace Api.Configurations;

public class PresentationServiceInstaller : IServiceInstaller
{
    public void Install(IServiceCollection services, IConfiguration configuration)
    {
        services.AddControllers()
            .AddApplicationPart(Presentation.AssemblyReference.Assembly);

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        services.AddApiVersioning(config =>
        {
            config.DefaultApiVersion = new ApiVersion(1, 0);

            config.AssumeDefaultVersionWhenUnspecified = true;

            config.ReportApiVersions = true;
        });

        services.AddTransient<GlobalExceptionHandlingMiddleware>();
    }
}
