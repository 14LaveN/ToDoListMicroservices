using System.Net;
using System.Security.Authentication;
using Microsoft.Extensions.Logging;
using ToDoListMicroservices.Application.ApiHelpers.Responses;
using ToDoListMicroservices.Application.Core.Abstractions.Messaging;
using ToDoListMicroservices.Domain.Common.Core.Errors;
using ToDoListMicroservices.Domain.Common.Core.Exceptions;
using ToDoListMicroservices.Domain.Common.Core.Primitives.Maybe;
using ToDoListMicroservices.Domain.Common.Core.Primitives.Result;
using ToDoListMicroservices.Identity.Contracts.Get;
using ToDoListMicroservices.Identity.Database.Data.Interfaces;

namespace ToDoListMicroservices.Identity.Application.Queries.GetTheUserById;

/// <summary>
/// Represents the <see cref="GetTheUserByIdQuery"/> handler.
/// </summary>
/// <param name="userRepository">The user repository.</param>
/// <param name="logger">The logger.</param>
internal sealed class GetTheUserByIdQueryHandler(
        IUserRepository userRepository,
        ILogger<GetTheUserByIdQueryHandler> logger)
    : IQueryHandler<GetTheUserByIdQuery, Maybe<GetUserResponse>>
{
    /// <inheritdoc />
    public async Task<Maybe<GetUserResponse>> Handle(
        GetTheUserByIdQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            logger.LogInformation($"Request for get an account by id - {request.UserId}");
            
            var user = await userRepository.GetByIdAsync(request.UserId);

            if (user.HasNoValue)
            {
                logger.LogWarning("User with the same identifier already taken");
                throw new NotFoundException(DomainErrors.User.NotFound, DomainErrors.User.NotFound);
            }
            
            logger.LogInformation($"Gets the user - {user.Value} {DateTime.UtcNow}");

            return Maybe<GetUserResponse>.From(new GetUserResponse(
                user.Value.FullName,
                user.Value.UserName!,
                user.Value.EmailAddress,
                user.Value.CreatedOnUtc));
        }
        catch (Exception exception)
        {
            logger.LogError(exception, $"[GetTheUserByIdQueryHandler]: {exception.Message}");
            return Maybe<GetUserResponse>.None;
        }
    }
}