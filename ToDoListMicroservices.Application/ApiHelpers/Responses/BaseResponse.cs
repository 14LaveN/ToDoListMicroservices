using System.Net;
using ToDoListMicroservices.Domain.Common.Core.Primitives.Result;

namespace ToDoListMicroservices.Application.ApiHelpers.Responses;

/// <summary>
/// Represents the base response class.
/// </summary>
/// <typeparam name="T">The generic result class.</typeparam>
public class BaseResponse<T> : IBaseResponse<T>
    where T : class
{
    /// <inheritdoc />
    public required string Description { get; set; }

    /// <inheritdoc />
    public Result<T> Data { get; set; }

    /// <inheritdoc />
    public required HttpStatusCode StatusCode { get; set; }
}