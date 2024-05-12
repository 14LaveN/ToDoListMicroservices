using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using ToDoListMicroservices.Domain.Core.Utility;
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
        Ensure.NotNull(services, "Services is required.", nameof(services));

        services
            .AddScoped<IValidator<ChangeNameCommand>, ChangeNameCommandValidator>()
            .AddScoped<IValidator<ChangePasswordCommand>, ChangePasswordCommandValidator>()
            .AddScoped<IValidator<LoginCommand>, LoginCommandValidator>()
            .AddScoped<IValidator<RegisterCommand>, RegisterCommandValidator>();
        
        return services;
    }
}