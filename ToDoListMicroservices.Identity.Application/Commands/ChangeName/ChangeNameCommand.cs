using ToDoListMicroservices.Application.ApiHelpers.Responses;
using ToDoListMicroservices.Application.Core.Abstractions.Messaging;
using ToDoListMicroservices.Domain.Common.Core.Primitives.Result;
using ToDoListMicroservices.Domain.Common.ValueObjects;
using ToDoListMicroservices.Identity.Domain.Entities;

namespace ToDoListMicroservices.Identity.Application.Commands.ChangeName;

/// <summary>
/// Represents the change <see cref="Name"/> command record.
/// </summary>
/// <param name="FirstName">The first name.</param>
/// <param name="LastName">The last name.</param>
public sealed record ChangeNameCommand(
    FirstName FirstName,
    LastName LastName)
    : ICommand<IBaseResponse<Result<User>>>;