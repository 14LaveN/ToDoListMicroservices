using System.Net;

namespace ToDoListMicroservices.Application.ApiHelpers.Responses;

/// <summary>
/// Represents the response from query class.
/// </summary>
/// <typeparam name="T">The generic type.</typeparam>
public sealed class QueryResponse<T>
    where T : class
{
    /// <summary>
    /// Gets or sets Status code.
    /// </summary>
    public HttpStatusCode StatusCode { get; set; }

    /// <summary>
    /// Gets or sets description.
    /// </summary>
    public string Description { get; set; }
    
    /// <summary>
    /// Gets or sets data.
    /// </summary>
    public T Data { get; set; }
}