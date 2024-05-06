


using ToDoListMicroservices.Domain.Common.Core.Events;
using ToDoListMicroservices.Identity.Domain.Events.User;

namespace ToDoListMicroservices.RabbitMq.Messaging.User.Events.UserCreated;

/// <summary>
/// Represents the <see cref="UserCreatedDomainEvent"/> handler.
/// </summary>
internal sealed class PublishIntegrationEventOnUserCreatedDomainEventHandler : IDomainEventHandler<UserCreatedDomainEvent>
{
    private readonly IIntegrationEventPublisher _integrationEventPublisher;

    /// <summary>
    /// Initializes a new instance of the <see cref="PublishIntegrationEventOnUserCreatedDomainEventHandler"/> class.
    /// </summary>
    /// <param name="integrationEventPublisher">The integration event publisher.</param>
    public PublishIntegrationEventOnUserCreatedDomainEventHandler(IIntegrationEventPublisher integrationEventPublisher) =>
        _integrationEventPublisher = integrationEventPublisher;

    /// <inheritdoc />
    public async System.Threading.Tasks.Task Handle(UserCreatedDomainEvent notification, CancellationToken cancellationToken) =>
        await _integrationEventPublisher.Publish(new UserCreatedIntegrationEvent(notification));
}