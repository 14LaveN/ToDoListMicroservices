using Microsoft.EntityFrameworkCore.Storage;
using ToDoListMicroservices.Database.Common.Abstractions;
using ToDoListMicroservices.Domain.Common.Core.Primitives;

namespace ToDoListMicroservices.Database.Common;

/// <summary>
/// Represents the generic unit of work.
/// </summary>
public sealed class UnitOfWork
    : IUnitOfWork
{
    private readonly BaseDbContext _baseDbContext;

    /// <summary>
    /// Initialize generic db context.
    /// </summary>
    /// <param name="baseDbContext">The base generic db context.</param>
    public UnitOfWork(BaseDbContext baseDbContext) =>
        _baseDbContext = baseDbContext;

    /// <summary>
    /// Save changes async in your db context
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The integer result of saving changes.</returns>
    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) =>
        await _baseDbContext.SaveChangesAsync(cancellationToken);

    /// <summary>
    /// Begin transaction async in your db context
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The db context transaction result of begin transaction.</returns>
    public async Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default) =>
        await _baseDbContext.Database.BeginTransactionAsync(cancellationToken);
}