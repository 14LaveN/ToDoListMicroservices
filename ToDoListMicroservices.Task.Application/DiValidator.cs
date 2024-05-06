using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace ToDoListMicroservices.Task.Application;

public static class DiValidator
{
    /// <summary>
    /// Registers the necessary services with the DI framework.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <returns>The same service collection.</returns>
    public static IServiceCollection AddValidators(this IServiceCollection services)
    {
        if (services is null)
        {
            throw new ArgumentNullException(nameof(services));
        }

        //services.AddScoped<IValidator<CreateProductCommand>, CreateProductCommandValidator>();
        
        return services;
    }
}