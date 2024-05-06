namespace ToDoListMicroservices.Notification.Domain.DTO;

/// <summary>
/// Represents the user answer dto record.
/// </summary>
/// <param name="Title">The title.</param>
/// <param name="UserName">The user name.</param>
/// <param name="TotalAnswers">The total answers.</param>
public record NotificationDto(
    string Title,
    string UserName,
    int TotalAnswers);