using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using ToDoListMicroservices.Contracts;
using ToDoListMicroservices.Database.Common;
using ToDoListMicroservices.Domain.Common.Core.Extensions;
using ToDoListMicroservices.Domain.Common.Core.Primitives.Maybe;
using ToDoListMicroservices.Domain.Common.Core.Primitives.Result;
using ToDoListMicroservices.Task.Database.Data.Interfaces;
using ToDoListMicroservices.Task.Domain.DTO.Tasks;
using ToDoListMicroservices.Task.Domain.Entities;

namespace ToDoListMicroservices.Task.Database.Data.Repositories;

/// <summary>
/// Represents the tasks repository.
/// </summary>
internal sealed class TasksRepository : GenericRepository<TaskEntity>, ITasksRepository
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TasksRepository"/> class.
    /// </summary>
    /// <param name="dbContext">The database context.</param>
    public TasksRepository(BaseDbContext dbContext)
        : base(dbContext)
    {
    }

    /// <inheritdoc />
    public async Task<Result<TaskEntity>> UpdateTask(TaskEntity task)
    {
        const string sql = """
                                           UPDATE dbo.tasks
                                           SET ModifiedOnUtc= @ModifiedOnUtc,
                                               Priority = @Priority,
                                               Title = @Title,
                                               Description = @Description
                                           WHERE Id = @Id AND Deleted = 0
                           """;
        
        SqlParameter[] parameters =
        {
            new("@ModifiedOnUtc", DateTime.UtcNow),
            new("@Title", task.Title.Value),
            new("@Id", task.Id),
            new("@Priority", task.Priority),
            new("@Description", task.Description)
        };
        int result = await DbContext.ExecuteSqlAsync(sql, parameters);
        
        return result is not 0 ? task : throw new ArgumentException();
    }

    /// <inheritdoc />
    public Task<PagedList<TasksDto>> GetAuthorTasksByIsDone(
        Guid authorId,
        int page,
        int pageSize) =>
            PagedList<TasksDto>.CreateAsync(
                GetAuthorTasksByIsDoneDelegate(DbContext, authorId),
                page,
                pageSize);

    private static readonly Func<BaseDbContext, Guid, IQueryable<TasksDto>> GetAuthorTasksByIsDoneDelegate =
            (Func<BaseDbContext, Guid, IQueryable<TasksDto>>)EF.CompileAsyncQuery(
                (BaseDbContext context, Guid authorId) =>
                    context.Set<TaskEntity>()
                        .AsSplitQuery()
                        .AsNoTracking()
                        .Include(x => x.Author)
                        .Include(x => x.PerformerOfWork)
                        .WhereIf(!authorId.Equals(Guid.Empty), t => t.AuthorId == authorId)
                        .Where(new DoneTasksSpecification())
                        .OrderByDescending(x => x.ModifiedOnUtc)
                        .Select(t => new TasksDto(
                            t.Author!.UserName!,
                            t.Description,
                            t.IsDone,
                            t.Title,
                            t.PerformerOfWork!.UserName!,
                            t.CreatedOnUtc,
                            t.Priority)));

}