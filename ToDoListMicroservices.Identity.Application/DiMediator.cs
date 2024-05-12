using MediatR.NotificationPublishers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using ToDoListMicroservices.Application.Core.Behaviours;
using ToDoListMicroservices.Domain.Core.Utility;
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
        Ensure.NotNull(services, "Services is required.", nameof(services));

        services.AddMediatR(x =>
        {
            x.RegisterServicesFromAssemblyContaining<Program>();

            x.RegisterServicesFromAssemblies(typeof(RegisterCommand).Assembly,
                    typeof(RegisterCommandHandler).Assembly)
                .RegisterServicesFromAssemblies(typeof(LoginCommand).Assembly,
                    typeof(LoginCommandHandler).Assembly)
                .RegisterServicesFromAssemblies(typeof(ChangePasswordCommand).Assembly,
                    typeof(ChangePasswordCommandHandler).Assembly)
                .RegisterServicesFromAssemblies(typeof(ChangeNameCommand).Assembly,
                    typeof(ChangeNameCommandHandler).Assembly)
                .RegisterServicesFromAssemblies(typeof(GetTheUserByIdQuery).Assembly,
                    typeof(GetTheUserByIdQueryHandler).Assembly);
            
            x.AddOpenBehavior(typeof(QueryCachingBehavior<,>))
                .AddOpenBehavior(typeof(UserTransactionBehavior<,>))
                .AddOpenBehavior(typeof(ValidationBehaviour<,>))
                .AddOpenBehavior(typeof(MetricsBehaviour<,>));
            
            x.NotificationPublisher = new TaskWhenAllPublisher();
            x.NotificationPublisherType = typeof(TaskWhenAllPublisher);
            x.Lifetime = ServiceLifetime.Scoped;
        });
        
        return services;
    }
}