using System.Reflection;
using Microsoft.Extensions.Configuration;
using ToDoListMicroservices.BackgroundTasks.QuartZ;
using ToDoListMicroservices.BackgroundTasks.QuartZ.Schedulers;
using ToDoListMicroservices.BackgroundTasks.Services;
using ToDoListMicroservices.BackgroundTasks.Settings;
using ToDoListMicroservices.BackgroundTasks.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Quartz.Spi;

namespace ToDoListMicroservices.BackgroundTasks;

public static class DependencyInjection
{
    /// <summary>
    /// Registers the necessary services with the DI framework.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="configuration">The configuration.</param>
    /// <returns>The same service collection.</returns>
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
            configure.UseMicrosoftDependencyInjectionJobFactory();
        });

        services.AddQuartzHostedService(options =>
        {
            options.WaitForJobsToComplete = true;
        });
        
        services.AddTransient<IJobFactory, QuartzJobFactory>();
        
        services.AddScoped<UserDbScheduler>();
        //services.AddSingleton<Firebase.Storage.FirebaseStorage>();
        
        //var scheduler = new UserDbScheduler();
        //scheduler.Start(services);
        
        return services;
    }
}