using Microsoft.Extensions.DependencyInjection;
using ToDoListMicroservices.Database.Common;
using ToDoListMicroservices.Notification.Database.Data.Interfaces;
using ToDoListMicroservices.Notification.Database.Data.Repositories;

namespace ToDoListMicroservices.Notification.Database;

public static class DependencyInjection
{
    /// <summary>
    /// Registers the necessary services with the DI framework.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <returns>The same service collection.</returns>
    public static IServiceCollection AddNotificationsDatabase(this IServiceCollection services)
    {
        if (services is null)
        {
            throw new ArgumentNullException(nameof(services));
        }

        services.AddTransient<BaseDbContext>();
        
        services.AddScoped<INotificationRepository, NotificationRepository>();

        return services;
    }
}