using Microsoft.Extensions.Caching.Distributed;
using ToDoListMicroservices.Database.Common;
using Quartz;
using ToDoListMicroservices.Identity.Database;
using ToDoListMicroservices.Identity.Domain.Entities;
using static System.Console;

namespace ToDoListMicroservices.BackgroundTasks.QuartZ.Jobs;

/// <summary>
/// Represents the base database job.
/// </summary>
public sealed class BaseDbJob : IJob
{
    private readonly BaseDbContext _appDbContext = new();

    /// <inheritdoc />
    public async System.Threading.Tasks.Task Execute(IJobExecutionContext context)
    {
        await _appDbContext.SaveChangesAsync();
        WriteLine($"All.SaveChanges - {DateTime.UtcNow}");
    }
}