using ToDoListMicroservices.Domain.Common.Core.Abstractions;
using ToDoListMicroservices.Domain.Common.Core.Errors;
using ToDoListMicroservices.Domain.Common.Core.Primitives;
using ToDoListMicroservices.Domain.Common.Core.Primitives.Result;
using ToDoListMicroservices.Domain.Common.ValueObjects;
using ToDoListMicroservices.Domain.Core.Primitives;
using ToDoListMicroservices.Domain.Core.Utility;
using ToDoListMicroservices.Identity.Domain.Entities;
using ToDoListMicroservices.Task.Domain.Enumerations;
using ToDoListMicroservices.Task.Domain.Events.Task;

namespace ToDoListMicroservices.Task.Domain.Entities;

/// <summary>
/// Represents the taskEntity entity class.
/// </summary>
public sealed class TaskEntity
    : AggregateRoot, IAuditableEntity, ISoftDeletableEntity
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TaskEntity"/> class.
    /// </summary>
    /// <param name="title">The title.</param>
    /// <param name="authorId">The taskEntity author identifier.</param>
    /// <param name="priority">The taskEntity priority.</param>
    /// <param name="createdAt">The created at date/time.</param>
    /// <param name="isDone">The is done checkbox.</param>
    /// <param name="description">The description.</param>
    /// <param name="performerOfWorkId">The performer of work identifier.</param>
    public TaskEntity(
        Name title,
        Guid authorId,
        TaskPriority priority,
        DateTime createdAt,
        bool isDone,
        string description,
        Guid performerOfWorkId = default)
    {
        Ensure.NotNull(title, "The name is required.", nameof(title));
        Ensure.NotEmpty(authorId, "The author identifier is required", nameof(authorId));
        Ensure.NotEmpty(createdAt, "The date and time is required.", nameof(createdAt));
        Ensure.NotNull(description, "The description is required.", nameof(description));
        
        IsDone = isDone;
        Title = title;
        AuthorId = authorId;
        Description = description;
        PerformerOfWorkId = performerOfWorkId;
        Priority = priority;
    }
    
    /// <summary>
    /// Gets or sets title.
    /// </summary>
    public Name Title { get; set; }
    
    /// <summary>
    /// Gets or sets author class.
    /// </summary>
    public User? Author { get; set; }

    /// <summary>
    /// Gets or sets performer of work user class.
    /// </summary>
    public User? PerformerOfWork { get; set; }
    
    /// <summary>
    /// Gets or sets author identifier.
    /// </summary>
    public Guid AuthorId { get; }
    
    /// <summary>
    /// Gets or sets Performer Of Work user identifier.
    /// </summary>
    public Guid PerformerOfWorkId { get; set; }

    /// <summary>
    /// Gets or sets is done checkbox.
    /// </summary>
    public bool IsDone { get; set; }
    
    /// <summary>
    /// Gets or sets description.
    /// </summary>
    public string Description { get; set; }
    
    /// <summary>
    /// Gets or sets taskEntity priority.
    /// </summary>
    public TaskPriority Priority { get; set; }

    /// <inheritdoc />
    public DateTime CreatedOnUtc { get; } = DateTime.UtcNow;
    
    /// <inheritdoc />
    public DateTime? ModifiedOnUtc { get; }
    
    /// <inheritdoc />
    public DateTime? DeletedOnUtc { get; }
    
    /// <inheritdoc />
    public bool Deleted { get; }

    /// <summary>
    /// Creates a new taskEntity with the specified author id, createdAt, title and description.
    /// </summary>
    /// <param name="authorId">The author id.</param>
    /// <param name="priority">The taskEntity priority.</param>
    /// <param name="description">The description.</param>
    /// <param name="title">The title.</param>
    /// <param name="companyId">The company identifier.</param>
    /// <returns>The newly created answer instance.</returns>
    public static TaskEntity Create(
        Guid authorId, 
        TaskPriority priority,
        string description,
        Name title)
    {
        var task = new TaskEntity(
            title,
            authorId,
            priority,
            DateTime.UtcNow, 
            false,
            description);

        task.AddDomainEvent(new TaskCreatedDomainEvent(task));
        return task;
    }
    
    /// <summary>
    /// Done taskEntity with <see cref="TaskEntity"/> and <see cref="User"/> classes.
    /// </summary>
    /// <param name="taskEntityEntity">The <see cref="TaskEntity"/> class.</param>
    /// <param name="user">The <see cref="User"/> class.</param>
    /// <returns>Return result.</returns>
    public async Task<Result> DoneTask(TaskEntity taskEntityEntity, User user)
    {
        if (IsDone)
        {
            return await Result.Failure(DomainErrors.Notification.AlreadySent);
        }

        IsDone = true;
        PerformerOfWorkId = user.Id;

        AddDomainEvent(new DoneTaskDomainEvent(taskEntityEntity, user));

        return await Result.Success();
    }
}