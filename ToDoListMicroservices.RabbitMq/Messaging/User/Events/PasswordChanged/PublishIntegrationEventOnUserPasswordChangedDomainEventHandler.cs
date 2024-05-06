


using ToDoListMicroservices.Domain.Common.Core.Events;
using ToDoListMicroservices.Identity.Domain.Events.User;

namespace ToDoListMicroservices.RabbitMq.Messaging.User.Events.PasswordChanged;

/// <summary>
/// Represents the <see cref="UserPasswordChangedDomainEvent"/> handler.
/// </summary>
internal sealed class PublishIntegrationEventOnUserPasswordChangedDomainEventHandler
    : IDomainEventHandler<UserPasswordChangedDomainEvent>
{
    private readonly IIntegrationEventPublisher _integrationEventPublisher;

    /// <summary>
    /// Initializes a new instance of the <see cref="PublishIntegrationEventOnUserPasswordChangedDomainEventHandler"/> class.
    /// </summary>
    /// <param name="integrationEventPublisher">The integration event publisher.</param>
    public PublishIntegrationEventOnUserPasswordChangedDomainEventHandler(IIntegrationEventPublisher integrationEventPublisher) =>
        _integrationEventPublisher = integrationEventPublisher;

    /// <inheritdoc />
    public async System.Threading.Tasks.Task Handle(UserPasswordChangedDomainEvent notification, CancellationToken cancellationToken)
    {
        await _integrationEventPublisher.Publish(new UserPasswordChangedIntegrationEvent(notification));

        await System.Threading.Tasks.Task.CompletedTask;
    }
}