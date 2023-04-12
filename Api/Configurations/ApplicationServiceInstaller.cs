using Application.Behaviors;
using FluentValidation;
using MediatR;

namespace Api.Configurations;

public class ApplicationServiceInstaller : IServiceInstaller
{
    public void Install(IServiceCollection services, IConfiguration configuration)
    {
        #region Add MediatR
        services.AddMediatR(Application.AssemblyReference.Assembly);
        #endregion

        #region Add AutoMapper
        services.AddAutoMapper(Application.AssemblyReference.Assembly);
        #endregion

        #region Add Fluent Validation
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationPipelineBehavior<,>));
        services.AddValidatorsFromAssembly(Application.AssemblyReference.Assembly,
            includeInternalTypes: true);
        #endregion
    }
}
