using ToDoListMicroservices.Application.Core.Abstractions.Messaging;

namespace ToDoListMicroservices.BackgroundTasks.Services;

/// <summary>
/// Represents the integration event consumer interface.
/// </summary>
internal interface IIntegrationEventConsumer
{
    /// <summary>
    /// Consumes the incoming the specified integration event.
    /// </summary>
    System.Threading.Tasks.Task Consume(IIntegrationEvent? integrationEvent);
}