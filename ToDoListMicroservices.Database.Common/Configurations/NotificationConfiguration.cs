using ToDoListMicroservices.Identity.Domain.Enumerations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using User = ToDoListMicroservices.Identity.Domain.Entities.User;

namespace ToDoListMicroservices.Database.Common.Configurations;

/// <summary>
/// Represents the configuration for the <see cref="Notification"/> entity.
/// </summary>
internal sealed class NotificationConfiguration : IEntityTypeConfiguration<Notification.Domain.Entities.Notification>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<Notification.Domain.Entities.Notification> builder)
    {
        builder.HasKey(notification => notification.Id);

        builder.HasOne<User>()
            .WithMany()
            .HasForeignKey(notification => notification.UserId)
            .IsRequired()
            .OnDelete(DeleteBehavior.NoAction);

        builder
            .Property(notification => notification.Sent)
            .IsRequired()
            .HasDefaultValue(false);

        builder
            .Property(notification => notification.DateTimeUtc)
            .IsRequired();

        builder
            .Property(invitation => invitation.CreatedOnUtc)
            .IsRequired();

        builder
            .Property(invitation => invitation.ModifiedOnUtc);

        builder
            .Property(invitation => invitation.DeletedOnUtc);

        builder
            .Property(invitation => invitation.Deleted)
            .HasDefaultValue(false);

        builder
            .HasQueryFilter(invitation => !invitation.Deleted);
    }
}