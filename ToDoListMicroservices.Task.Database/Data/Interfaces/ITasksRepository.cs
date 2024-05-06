using ToDoListMicroservices.Contracts;
using ToDoListMicroservices.Domain.Common.Core.Primitives.Maybe;
using ToDoListMicroservices.Domain.Common.Core.Primitives.Result;
using ToDoListMicroservices.Task.Domain.DTO.Tasks;

namespace ToDoListMicroservices.Task.Database.Data.Interfaces;

/// <summary>
/// Represents the tasks repository interface.
/// </summary>
public interface ITasksRepository
{
    /// <summary>
    /// Gets the taskEntity with the specified identifier.
    /// </summary>
    /// <param name="taskId">The taskEntity identifier.</param>
    /// <returns>The maybe instance that may contain the taskEntity entity with the specified identifier.</returns>
    Task<Maybe<Domain.Entities.TaskEntity>> GetByIdAsync(Guid taskId);

    /// <summary>
    /// Inserts the specified taskEntity entity to the database.
    /// </summary>
    /// <param name="taskEntity">The taskEntity to be inserted to the database.</param>
    Task<Result> Insert(Domain.Entities.TaskEntity taskEntity);

    /// <summary>
    /// Remove the specified taskEntity entity to the database.
    /// </summary>
    /// <param name="taskEntity">The taskEntity to be inserted to the database.</param>
    void Remove(Domain.Entities.TaskEntity taskEntity);

    /// <summary>
    /// Update the specified taskEntity entity to the database.
    /// </summary>
    /// <param name="taskEntity">The taskEntity to be inserted to the database.</param>
    /// <returns>The result instance that may contain the taskEntity entity with the specified taskEntity class.</returns>
    Task<Result<Domain.Entities.TaskEntity>> UpdateTask(Domain.Entities.TaskEntity taskEntity);

    /// <summary>
    /// Gets the paged list tasks with the specified author identifier.
    /// </summary>
    /// <param name="authorId">The author identifier.</param>
    /// <param name="page">The page.</param>
    /// <param name="pageSize">The page size.</param>
    /// <returns>The maybe instance that may contain the paged list taskEntity DTO with the specified taskEntity class.</returns>
    Task<PagedList<TasksDto>> GetAuthorTasksByIsDone(
        Guid authorId,
        int page,
        int pageSize);
}