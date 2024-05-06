using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using ToDoListMicroservices.Database.Common;
using ToDoListMicroservices.Identity.Domain.Entities;
using ToDoListMicroservices.Notification.Database.Data.Interfaces;

namespace ToDoListMicroservices.Notification.Database.Data.Repositories;

/// <summary>
/// Represents the notification repository.
/// </summary>
internal sealed class NotificationRepository : GenericRepository<ToDoListMicroservices.Notification.Domain.Entities.Notification>, INotificationRepository
{
    /// <summary>
    /// Initializes a new instance of the <see cref="NotificationRepository"/> class.
    /// </summary>
    /// <param name="dbContext">The database context.</param>
    public NotificationRepository(
        BaseDbContext dbContext)
        : base(dbContext)
    {
    }
}