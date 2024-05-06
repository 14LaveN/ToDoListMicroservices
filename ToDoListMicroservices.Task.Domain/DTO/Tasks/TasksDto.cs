using ToDoListMicroservices.Task.Domain.Enumerations;

namespace ToDoListMicroservices.Task.Domain.DTO.Tasks;

/// <summary>
/// Represents the tasks dto record.
/// </summary>
/// <param name="AuthorName">The author name.</param>
/// <param name="Description">The description.</param>
/// <param name="IsDone">The is done flag.</param>
/// <param name="Title">The title.</param>
/// <param name="PerformerOfWork">The performer of work name.</param>
/// <param name="CreatedAt">The date/time creation.</param>
/// <param name="TaskPriority">The taskEntity priority.</param>
public sealed record TasksDto(
    string AuthorName,
    string Description,
    bool IsDone,
    string Title,
    string PerformerOfWork,
    DateTime CreatedAt,
    TaskPriority TaskPriority);