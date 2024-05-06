using ToDoListMicroservices.Domain.Common.Core.Events;

namespace ToDoListMicroservices.Task.Domain.Events.Task;

/// <summary>
/// Represents the event that is raised when a taskEntity is created.
/// </summary>
public sealed class TaskCreatedDomainEvent 
    : IDomainEvent
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TaskCreatedDomainEvent"/> class.
    /// </summary>
    /// <param name="taskEntity">The taskEntity entity.</param>
    internal TaskCreatedDomainEvent(Entities.TaskEntity taskEntity) => 
        TaskEntity = taskEntity;

    /// <summary>
    /// Gets the user.
    /// </summary>
    public Entities.TaskEntity TaskEntity { get; }
}