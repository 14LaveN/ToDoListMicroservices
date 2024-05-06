using ToDoListMicroservices.Application.Core.Abstractions.Messaging;
using ToDoListMicroservices.Domain.Common.Core.Primitives.Result;
using ToDoListMicroservices.Identity.Domain.Entities;

namespace ToDoListMicroservices.Identity.Application.Commands.ChangePassword;

/// <summary>
/// Represents the change password command record class.
/// </summary>
/// <param name="Password">The password.</param>
public sealed record ChangePasswordCommand(
    string Password) : ICommand<Result<User>>;