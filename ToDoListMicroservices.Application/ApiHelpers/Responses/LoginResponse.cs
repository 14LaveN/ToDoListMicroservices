using System.Net;
using ToDoListMicroservices.Domain.Common.Core.Primitives.Result;
using ToDoListMicroservices.Identity.Domain.Entities;

namespace ToDoListMicroservices.Application.ApiHelpers.Responses;

/// <summary>
/// Gets or sets login response class.
/// </summary>
/// <typeparam name="T">The generic type.</typeparam>
public class LoginResponse<T> 
{
    /// <summary>
    /// Gets or sets description.
    /// </summary>
    public required string Description { get; set; } = null!;

    /// <summary>
    /// Gets or sets data.
    /// </summary>
    public Task<Result<User>> Data { get; set; }

    /// <summary>
    /// Gets or sets status code.
    /// </summary>
    public required HttpStatusCode StatusCode { get; set; }
    
    /// <summary>
    /// Gets or sets access token.
    /// </summary>
    public string AccessToken { get; set; } = null!;
    
    /// <summary>
    /// Gets or sets refresh token.
    /// </summary>
    public string RefreshToken { get; set; } = null!;
    
    /// <summary>
    /// Gets or sets date/time refresh token expire at.
    /// </summary>
    public DateTime RefreshTokenExpireAt { get; set; }
}