using ToDoListMicroservices.Identity.Domain.Events.User;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ToDoListMicroservices.RabbitMq.Messaging;
using ToDoListMicroservices.RabbitMq.Messaging.Settings;
using ToDoListMicroservices.RabbitMq.Messaging.User.Events.UserCreated;

namespace ToDoListMicroservices.RabbitMq;

public static class DependencyInjection
{
    public static IServiceCollection AddRabbitMq(this IServiceCollection services,
        IConfiguration configuration)
    {
        if (services is null)
        {
            throw new ArgumentNullException(nameof(services));
        }

        services.AddMediatR(x =>
        {
            x.RegisterServicesFromAssemblies(typeof(UserCreatedDomainEvent).Assembly,
                typeof(PublishIntegrationEventOnUserCreatedDomainEventHandler).Assembly);
        });
        
        services.Configure<MessageBrokerSettings>(configuration.GetSection(MessageBrokerSettings.SettingsKey));
        
        services.AddScoped<IIntegrationEventPublisher, IntegrationEventPublisher>();

        services.AddHealthChecks()
            .AddRabbitMQ(new Uri(MessageBrokerSettings.AmqpLink));
        
        return services; 
    }
}