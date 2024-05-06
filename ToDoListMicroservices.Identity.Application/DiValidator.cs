using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using ToDoListMicroservices.Identity.Application.Commands.ChangeName;
using ToDoListMicroservices.Identity.Application.Commands.ChangePassword;
using ToDoListMicroservices.Identity.Application.Commands.Login;
using ToDoListMicroservices.Identity.Application.Commands.Register;

namespace ToDoListMicroservices.Identity.Application;

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

        services.AddScoped<IValidator<ChangeNameCommand>, ChangeNameCommandValidator>();
        services.AddScoped<IValidator<ChangePasswordCommand>, ChangePasswordCommandValidator>();
        services.AddScoped<IValidator<LoginCommand>, LoginCommandValidator>();
        services.AddScoped<IValidator<RegisterCommand>, RegisterCommandValidator>();
        
        return services;
    }
}