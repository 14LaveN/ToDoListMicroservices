using ToDoListMicroservices.Application.Core.Abstractions.Messaging;
using ToDoListMicroservices.Domain.Common.Core.Primitives.Maybe;
using ToDoListMicroservices.Identity.Contracts.Get;

namespace ToDoListMicroservices.Identity.Application.Queries.GetTheUserById;

/// <summary>
/// Represents the get user by id query record.
/// </summary>
/// <param name="UserId">The user identifier.</param>
public sealed record GetTheUserByIdQuery(Guid UserId)
    : ICachedQuery<Maybe<GetUserResponse>>
{
    public string Key { get; } = $"get-user-by-{UserId}";
    
    public TimeSpan? Expiration { get; } = TimeSpan.FromMinutes(6);
}