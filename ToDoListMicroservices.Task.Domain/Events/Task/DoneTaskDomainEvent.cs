using ToDoListMicroservices.Domain.Common.Core.Events;
using ToDoListMicroservices.Identity.Domain.Entities;

namespace ToDoListMicroservices.Task.Domain.Events.Task;

/// <summary>
/// Represents the event that is raised when the taskEntity is done.
/// </summary>
public sealed class DoneTaskDomainEvent
    : IDomainEvent
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DoneTaskDomainEvent"/> class.
    /// </summary>
    /// <param name="taskEntity">The taskEntity entity.</param>
    /// <param name="user">The user.</param>
    internal DoneTaskDomainEvent(
        Entities.TaskEntity taskEntity,
        User user) =>
        (TaskEntity, User) = 
        (taskEntity,user);

    /// <summary>
    /// Gets the taskEntity entity.
    /// </summary>
    public Entities.TaskEntity TaskEntity { get; }

    /// <summary>
    /// Gets the user entity.
    /// </summary>
    public User User { get; }
}