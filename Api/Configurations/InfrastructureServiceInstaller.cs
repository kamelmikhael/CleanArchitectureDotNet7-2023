using Infrastructure.BackgroundJobs;
using Infrastructure.Contexts;
using Infrastructure.Idempotence;
using Infrastructure.Interceptors;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Quartz;

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

        services.Decorate(typeof(INotificationHandler<>), typeof(IdempotentDomainEventHandler<>));

        //services.AddSingleton<ConvertDomainEventsToOutboxMessagesInterceptor>();
        //services.AddSingleton<AuditingInterceptor>();

        services.AddDbContext<ApplicationDbContext>((sp, options) =>
        {
            //var outboxMessagesInterceptor = sp.GetService<ConvertDomainEventsToOutboxMessagesInterceptor>();
            //var auditingInterceptor = sp.GetService<AuditingInterceptor>();

            options
                .UseSqlServer(
                    configuration.GetConnectionString("DefaultConnection"),
                    b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName));
                //.AddInterceptors(outboxMessagesInterceptor);
        });

        services.AddQuartz(configure =>
        {
            var jobKey = new JobKey(nameof(ProcessOutboxMessagesJob));

            configure
                .AddJob<ProcessOutboxMessagesJob>(jobKey)
                .AddTrigger(trigger =>
                    trigger.ForJob(jobKey)
                            .WithSimpleSchedule(schedule =>
                                schedule.WithIntervalInSeconds(10).RepeatForever()
                            )
                );

            configure.UseMicrosoftDependencyInjectionJobFactory();
        });

        services.AddQuartzHostedService();
    }
}
