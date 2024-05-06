using ToDoListMicroservices.Identity.Domain.Entities;

namespace ToDoListMicroservices.Notification.Database.Data.Interfaces;

/// <summary>
/// Represents the notifications repository.
/// </summary>
public interface INotificationRepository
{
    /// <summary>
    /// Inserts the specified notifications to the database.
    /// </summary>
    /// <param name="notifications">The notifications to be inserted into the database.</param>
    System.Threading.Tasks.Task InsertRange(IReadOnlyCollection<Domain.Entities.Notification> notifications);

    /// <summary>
    /// Updates the specified notification in the database.
    /// </summary>
    /// <param name="notification">The notification to be updated.</param>
    void Update(Domain.Entities.Notification notification);
}