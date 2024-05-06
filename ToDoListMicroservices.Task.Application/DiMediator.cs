using MediatR.NotificationPublishers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using ToDoListMicroservices.Application.Core.Behaviours;

namespace ToDoListMicroservices.Task.Application;

public static class DiMediator
{
    /// <summary>
    /// Registers the necessary services with the DI framework.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <returns>The same service collection.</returns>
    public static IServiceCollection AddMediatr(this IServiceCollection services)
    {
        if (services is null)
        {
            throw new ArgumentNullException(nameof(services));
        }

        services.AddMediatR(x =>
        {
            x.RegisterServicesFromAssemblyContaining<Program>();
            
            x.AddOpenBehavior(typeof(QueryCachingBehavior<,>));
            x.AddOpenBehavior(typeof(BaseTransactionBehavior<,>));
            x.AddOpenBehavior(typeof(ValidationBehaviour<,>));
            x.AddOpenBehavior(typeof(MetricsBehaviour<,>));
            
            x.NotificationPublisher = new TaskWhenAllPublisher();
        });
        
        return services;
    }
}