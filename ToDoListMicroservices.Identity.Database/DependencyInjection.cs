using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ToDoListMicroservices.Identity.Database.Data.Interfaces;
using ToDoListMicroservices.Identity.Database.Data.Repositories;

namespace ToDoListMicroservices.Identity.Database;

public static class DependencyInjection
{
    /// <summary>
    /// Registers the necessary services with the DI framework.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <returns>The same service collection.</returns>
    public static IServiceCollection AddUserDatabase(this IServiceCollection services)
    {
        if (services is null)
        {
            throw new ArgumentNullException(nameof(services));
        }
        
        services.AddScoped<IUserUnitOfWork, UserUnitOfWork>();
        services.AddScoped<IUserRepository, UserRepository>();
        
        return services;
    }
}