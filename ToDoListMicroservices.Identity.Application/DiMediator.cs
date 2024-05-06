using MediatR.NotificationPublishers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using ToDoListMicroservices.Application.Core.Behaviours;
using ToDoListMicroservices.Identity.Application.Behaviour;
using ToDoListMicroservices.Identity.Application.Commands.ChangeName;
using ToDoListMicroservices.Identity.Application.Commands.ChangePassword;
using ToDoListMicroservices.Identity.Application.Commands.Login;
using ToDoListMicroservices.Identity.Application.Commands.Register;
using ToDoListMicroservices.Identity.Application.Queries.GetTheUserById;

namespace ToDoListMicroservices.Identity.Application;

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

            x.RegisterServicesFromAssemblies(typeof(RegisterCommand).Assembly,
                typeof(RegisterCommandHandler).Assembly);
            
            x.RegisterServicesFromAssemblies(typeof(LoginCommand).Assembly,
                typeof(LoginCommandHandler).Assembly);
            
            x.RegisterServicesFromAssemblies(typeof(ChangePasswordCommand).Assembly,
                typeof(ChangePasswordCommandHandler).Assembly);
            
            x.RegisterServicesFromAssemblies(typeof(ChangeNameCommand).Assembly,
                typeof(ChangeNameCommandHandler).Assembly);

            x.RegisterServicesFromAssemblies(typeof(GetTheUserByIdQuery).Assembly,
                typeof(GetTheUserByIdQueryHandler).Assembly);
            
            x.AddOpenBehavior(typeof(QueryCachingBehavior<,>));
            x.AddOpenBehavior(typeof(UserTransactionBehavior<,>));
            x.AddOpenBehavior(typeof(ValidationBehaviour<,>));
            x.AddOpenBehavior(typeof(MetricsBehaviour<,>));
            
            x.NotificationPublisher = new TaskWhenAllPublisher();
        });
        
        return services;
    }
}