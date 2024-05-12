using ToDoListMicroservices.Application.ApiHelpers.Contracts;
using ToDoListMicroservices.Application.ApiHelpers.Infrastructure;
using ToDoListMicroservices.Application.ApiHelpers.Policy;
using ToDoListMicroservices.Domain.Common.Core.Errors;
using ToDoListMicroservices.Domain.Common.Core.Primitives.Result;
using ToDoListMicroservices.Domain.Common.ValueObjects;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using ToDoListMicroservices.Identity.Application.ApiHelpers.Infrastructure;
using ToDoListMicroservices.Identity.Application.Commands.ChangeName;
using ToDoListMicroservices.Identity.Application.Commands.ChangePassword;
using ToDoListMicroservices.Identity.Application.Commands.Login;
using ToDoListMicroservices.Identity.Application.Commands.Register;
using ToDoListMicroservices.Identity.Contracts.ChangeName;
using ToDoListMicroservices.Identity.Database.Data.Interfaces;

namespace ToDoListMicroservices.Identity.Api.Controllers.V1;

/// <summary>
/// Represents the users controller class.
/// </summary>
/// <param name="sender">The sender.</param>
/// <param name="userRepository">The user repository.</param>
[Route("api/v1/users")]
public sealed class UsersController(
        ISender sender,
        IUserRepository userRepository)
    : IdentityApiController(sender, userRepository)
{
    #region Commands.
    
    /// <summary>
    /// Login user.
    /// </summary>
    /// <param name="request">The <see cref="LoginRequest"/> class.</param>
    /// <returns>Base information about login user method.</returns>
    /// <remarks>
    /// Example request:
    /// </remarks>
    /// <response code="200">OK.</response>
    /// <response code="401">Unauthorized.</response>
    /// <response code="500">Internal server error.</response>
    [HttpPost(ApiRoutes.Users.Login)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login([FromBody] Contracts.Login.LoginRequest request) =>
        await Result.Create(request, DomainErrors.General.UnProcessableRequest)
            .Map(loginRequest => new LoginCommand(loginRequest.UserName,Password.Create(loginRequest.Password).Value))
            .Bind(command => BaseRetryPolicy.Policy.Execute(async () =>
                await Sender.Send(command)).Result.Data)
            .Match(Ok, Unauthorized);
    
    /// <summary>
    /// Register user.
    /// </summary>
    /// <param name="request">The <see cref="RegisterRequest"/> class.</param>
    /// <returns>Base information about register user method.</returns>
    /// <remarks>
    /// Example request:
    /// </remarks>
    /// <response code="200">OK.</response>
    /// <response code="401">Unauthorized.</response>
    /// <response code="500">Internal server error.</response>
    [HttpPost(ApiRoutes.Users.Register)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Register([FromBody] Contracts.Register.RegisterRequest request) =>
        await Result.Create(request, DomainErrors.General.UnProcessableRequest)
            .Map(registerRequest => new RegisterCommand(
                    FirstName.Create(registerRequest.FirstName).Value,
                    LastName.Create(registerRequest.LastName).Value,
                    new EmailAddress(registerRequest.Email),
                    Password.Create(registerRequest.Password).Value,
                    registerRequest.UserName))
            .Bind(command => BaseRetryPolicy.Policy.Execute(async () =>
                await Sender.Send(command)).Result.Data)
            .Match(Ok, Unauthorized);

    /// <summary>
    /// Change password from user.
    /// </summary>
    /// <param name="password">The password.</param>
    /// <returns>Base information about change password from user method.</returns>
    /// <remarks>
    /// Example request:
    /// </remarks>
    /// <response code="200">OK.</response>
    /// <response code="400">Bad request.</response>
    /// <response code="500">Internal server error.</response>
    [HttpPut(ApiRoutes.Users.ChangePassword)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> ChangePassword([FromBody] string password) =>
        await Result.Create(password, DomainErrors.General.UnProcessableRequest)
            .Map(changePasswordRequest => new ChangePasswordCommand(changePasswordRequest))
            .Bind(command => BaseRetryPolicy.Policy.Execute(async () =>
                await Sender.Send(command)))
            .Match(Ok, BadRequest);
    
    /// <summary>
    /// Change name from user.
    /// </summary>
    /// <param name="request">The request.</param>
    /// <returns>Base information about change name from user method.</returns>
    /// <remarks>
    /// Example request:
    /// </remarks>
    /// <response code="200">OK.</response>
    /// <response code="400">Bad request.</response>
    /// <response code="500">Internal server error.</response>
    [HttpPut(ApiRoutes.Users.ChangeName)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> ChangeName([FromBody] ChangeNameRequest request) =>
        await Result.Create(request, DomainErrors.General.UnProcessableRequest)
            .Map(changeNameRequest => new ChangeNameCommand(
                FirstName.Create(changeNameRequest.FirstName).Value,
                LastName.Create(changeNameRequest.LastName).Value))
            .Bind(command => System.Threading.Tasks.Task.FromResult(BaseRetryPolicy.Policy.Execute(async () =>
                await Sender.Send(command)).Result.Data))
            .Match(Ok, BadRequest);
    
    #endregion
}