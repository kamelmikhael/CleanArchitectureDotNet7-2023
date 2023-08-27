using Api.Options;
using Api.OptionsSetup;
using Infrastructure.BackgroundJobs;
using Infrastructure.Contexts;
using Infrastructure.Idempotence;
using Infrastructure.Interceptors;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
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

        services.ConfigureOptions<DatabaseOptionsSetup>();

        services.AddDbContext<ApplicationDbContext>((serviceProvider, dbContextOptionsBuilder) =>
        {
            var databaseOptions = serviceProvider.GetService<IOptions<DatabaseOptions>>()!.Value;
            //var outboxMessagesInterceptor = serviceProvider.GetService<ConvertDomainEventsToOutboxMessagesInterceptor>();
            //var auditingInterceptor = serviceProvider.GetService<AuditingInterceptor>();

            dbContextOptionsBuilder
                .UseSqlServer(
                    databaseOptions.ConnectionString,
                    sqlServerAction =>
                    {
                        sqlServerAction.EnableRetryOnFailure(databaseOptions.MaxRetryCount);

                        sqlServerAction.CommandTimeout(databaseOptions.CommandTimeout);

                        sqlServerAction.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName);
                    });
                //.AddInterceptors(outboxMessagesInterceptor);

            dbContextOptionsBuilder.EnableDetailedErrors(databaseOptions.EnabledDetailedErrors);

            dbContextOptionsBuilder.EnableSensitiveDataLogging(databaseOptions.EnabledSensitiveDataLogging);
        });

        services.AddQuartz(configure =>
        {
            var jobKey = new JobKey(nameof(ProcessOutboxMessagesJob));

            configure
                .AddJob<ProcessOutboxMessagesJob>(jobKey)
                .AddTrigger(trigger =>
                    trigger.ForJob(jobKey)
                            .WithSimpleSchedule(schedule =>
                                schedule.WithIntervalInSeconds(60).RepeatForever()
                            )
                );

            configure.UseMicrosoftDependencyInjectionJobFactory();
        });

        services.AddQuartzHostedService();
    }
}
