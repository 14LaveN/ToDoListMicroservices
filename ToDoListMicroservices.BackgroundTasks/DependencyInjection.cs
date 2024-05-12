using System.Reflection;
using Microsoft.Extensions.Configuration;
using ToDoListMicroservices.BackgroundTasks.QuartZ;
using ToDoListMicroservices.BackgroundTasks.QuartZ.Schedulers;
using ToDoListMicroservices.BackgroundTasks.Services;
using ToDoListMicroservices.BackgroundTasks.Settings;
using ToDoListMicroservices.BackgroundTasks.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Quartz;
using Quartz.Spi;
using ToDoListMicroservices.BackgroundTasks.QuartZ.Jobs;

namespace ToDoListMicroservices.BackgroundTasks;

public static class DependencyInjection
{
    /// <summary>
    /// Registers the necessary services with the DI framework.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="configuration">The configuration.</param>
    /// <returns>The same service collection.</returns>
    [Obsolete("Obsolete")]
    public static IServiceCollection AddBackgroundTasks(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddMediatR(x=>
            x.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

        services.Configure<BackgroundTaskSettings>(configuration.GetSection(BackgroundTaskSettings.SettingsKey));

        services.AddHostedService<IntegrationEventConsumerBackgroundService>();
        services.AddHostedService<SaveMetricsBackgroundService>();
        services.AddHostedService<CreateReportBackgroundService>();
        
        services.AddScoped<IIntegrationEventConsumer, IntegrationEventConsumer>();
        services.AddScoped<ICreateReportProducer, CreateReportProducer>();
        
        services.AddQuartz(configure =>
        {
            #region BaseDbJobSetup

            var jobKey = new JobKey(
                nameof(BaseDbJob));

            configure
                .AddJob<BaseDbJob>(jobKey)
                .AddTrigger(
                    trigger => trigger
                        .ForJob(jobKey)
                        .WithSimpleSchedule(schedule =>
                            schedule
                                .WithIntervalInSeconds(120)
                                .RepeatForever()));


            #endregion
            
            configure.UseMicrosoftDependencyInjectionJobFactory();
        });

        services.AddQuartzHostedService(options =>
        {
            options.WaitForJobsToComplete = true;
        });

        services.Configure<HostOptions>(options =>
        {
            options.ServicesStartConcurrently = true;
            options.ServicesStopConcurrently = true;
        });
        
        services.AddTransient<IJobFactory, QuartzJobFactory>();
        
        services.AddScoped<UserDbScheduler>();
        
        return services;
    }
}