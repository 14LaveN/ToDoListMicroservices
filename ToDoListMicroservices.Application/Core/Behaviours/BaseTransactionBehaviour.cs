using ToDoListMicroservices.Application.Core.Abstractions.Messaging;
using ToDoListMicroservices.Database.Common;
using ToDoListMicroservices.Database.Common.Abstractions;
using MediatR;
using ToDoListMicroservices.Identity.Database.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ToDoListMicroservices.Application.Core.Behaviours;

/// <summary>
/// Represents the generic transaction behaviour class.
/// </summary>
public sealed class BaseTransactionBehavior<TRequest, TResponse> 
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : ICommand<TResponse>
    where TResponse : class
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly BaseDbContext _userDbContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="BaseTransactionBehavior{TRequest,TResponse}"/> class.
    /// </summary>
    /// <param name="unitOfWork">The user unit of work.</param>
    /// <param name="userDbContext">The user database context.</param>
    public BaseTransactionBehavior(
        IUnitOfWork unitOfWork,
        BaseDbContext userDbContext)
    {
        _unitOfWork = unitOfWork;
        _userDbContext = userDbContext;
    }

    /// <inheritdoc/>
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (request is IQuery<TResponse>)
        {
            return await next();
        }
        
        await using var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            TResponse response = await next();

            await transaction.CommitAsync(cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return response;
        }
        catch (Exception)
        {
            await transaction.RollbackAsync(cancellationToken);

            throw;
        }
    }
}