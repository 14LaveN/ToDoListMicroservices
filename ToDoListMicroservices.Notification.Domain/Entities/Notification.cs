using ToDoListMicroservices.Domain.Common.Core.Abstractions;
using ToDoListMicroservices.Domain.Common.Core.Errors;
using ToDoListMicroservices.Domain.Common.Core.Primitives;
using ToDoListMicroservices.Domain.Common.Core.Primitives.Result;
using ToDoListMicroservices.Domain.Core.Utility;
using ToDoListMicroservices.Identity.Domain.Entities;
using ToDoListMicroservices.Identity.Domain.Enumerations;

namespace ToDoListMicroservices.Notification.Domain.Entities;

/// <summary>
/// Represents the notification.
/// </summary>
public sealed class Notification
    : Entity, IAuditableEntity, ISoftDeletableEntity
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Notification"/> class.
    /// </summary>
    /// <param name="userId">The user identifier.</param>
    /// <param name="dateTimeUtc">The date and time of the notification.</param>
    internal Notification(Guid userId, DateTime dateTimeUtc)
        : base(Guid.NewGuid())
    {
        Ensure.NotEmpty(userId, "The user identifier is required.", nameof(userId));
        Ensure.NotEmpty(dateTimeUtc, "The date and time is required.", nameof(dateTimeUtc));
        
        UserId = userId;
        DateTimeUtc = dateTimeUtc;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Notification"/> class.
    /// </summary>
    /// <remarks>
    /// Required by EF Core.
    /// </remarks>
    private Notification()
    {
    }

    /// <summary>
    /// Gets the user identifier.
    /// </summary>
    public Guid UserId { get; private set; }

    /// <summary>
    /// Gets the date and time in UTC format of when the notification is supposed to be sent.
    /// </summary>
    public DateTime DateTimeUtc { get; private set; }

    /// <summary>
    /// Gets the value indicating whether or not the notification has been sent.
    /// </summary>
    public bool Sent { get; private set; }

    /// <inheritdoc />
    public DateTime CreatedOnUtc { get; }

    /// <inheritdoc />
    public DateTime? ModifiedOnUtc { get; }

    /// <inheritdoc />
    public DateTime? DeletedOnUtc { get; }

    /// <inheritdoc />
    public bool Deleted { get; }

    /// <summary>
    /// Marks the notification as sent and returns the respective result.
    /// </summary>
    /// <returns>The success result if the notification was not previously marked as sent, otherwise a failure result.</returns>
    public Result MarkAsSent()
    {
        if (Sent)
        {
            return Result.Failure(DomainErrors.Notification.AlreadySent).GetAwaiter().GetResult();
        }

        Sent = true;

        return Result.Success().GetAwaiter().GetResult();
    }
}