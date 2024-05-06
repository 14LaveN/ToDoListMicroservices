using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using ToDoListMicroservices.BackgroundTasks.Services;
using ToDoListMicroservices.BackgroundTasks.Tasks;
using ToDoListMicroservices.RabbitMq.Messaging.Settings;
using Microsoft.Extensions.Configuration;

namespace ToDoListMicroservices.BackgroundTasks;

public static class DiIntegrationEvent
{
    /// <summary>
    /// Registers the necessary services with the DI framework.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <returns>The same service collection.</returns>
    public static IServiceCollection AddRabbitBackgroundTasks(
        this IServiceCollection services)
    {
        services.AddMediatR(x=>
            x.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
        
        services.AddHostedService<IntegrationEventConsumerBackgroundService>();

        services.AddScoped<IIntegrationEventConsumer, IntegrationEventConsumer>();

        return services;
    }
}