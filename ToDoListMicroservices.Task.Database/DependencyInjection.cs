using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ToDoListMicroservices.Task.Database.Data.Interfaces;
using ToDoListMicroservices.Task.Database.Data.Repositories;

namespace ToDoListMicroservices.Task.Database;

public static class DependencyInjection
{
    /// <summary>
    /// Registers the necessary services with the DI framework.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <returns>The same service collection.</returns>
    public static IServiceCollection AddTasksDatabase(this IServiceCollection services)
    {
        if (services is null)
        {
            throw new ArgumentNullException(nameof(services));
        }
        
        services.AddScoped<ITasksRepository, TasksRepository>();

        return services;
    }
}