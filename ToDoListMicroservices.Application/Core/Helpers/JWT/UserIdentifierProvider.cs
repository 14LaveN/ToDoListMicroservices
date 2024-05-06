using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using ToDoListMicroservices.Application.Core.Abstractions.Helpers.JWT;

namespace ToDoListMicroservices.Application.Core.Helpers.JWT;

/// <summary>
/// Represents the user identifier provider.
/// </summary>
public sealed class UserIdentifierProvider : IUserIdentifierProvider
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UserIdentifierProvider"/> class.
    /// </summary>
    /// <param name="httpContextAccessor">The HTTP context accessor.</param>
    public UserIdentifierProvider(IHttpContextAccessor httpContextAccessor)
    {
        string userId = httpContextAccessor.HttpContext?.User.FindFirst("nameId")?.Value
                             ?? GetClaimByJwtToken.GetIdByToken(httpContextAccessor.HttpContext?.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last());
        
        UserId = new Guid(userId);
    }

    /// <inheritdoc />
    public Guid UserId { get; }
}