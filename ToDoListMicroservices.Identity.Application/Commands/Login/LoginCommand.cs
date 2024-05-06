using ToDoListMicroservices.Application.ApiHelpers.Responses;
using ToDoListMicroservices.Application.Core.Abstractions.Messaging;
using ToDoListMicroservices.Domain.Common.Core.Primitives.Result;
using ToDoListMicroservices.Domain.Common.ValueObjects;
using ToDoListMicroservices.Identity.Domain.Entities;

namespace ToDoListMicroservices.Identity.Application.Commands.Login;

/// <summary>
/// Represents the login command record class.
/// </summary>
/// <param name="UserName">The user name.</param>
/// <param name="Password">The password.</param>
public sealed record LoginCommand(
        string UserName,
        Password Password)
    : ICommand<LoginResponse<Result<User>>>;