using System.Net;
using System.Security.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ToDoListMicroservices.Application.ApiHelpers.Responses;
using ToDoListMicroservices.Application.Core.Abstractions.Messaging;
using ToDoListMicroservices.Domain.Common.Core.Errors;
using ToDoListMicroservices.Domain.Common.Core.Exceptions;
using ToDoListMicroservices.Domain.Common.Core.Primitives.Result;
using ToDoListMicroservices.Domain.Common.ValueObjects;
using ToDoListMicroservices.Identity.Application.Core.Settings.User;
using ToDoListMicroservices.Identity.Application.Extensions;
using ToDoListMicroservices.Identity.Domain.Entities;

namespace ToDoListMicroservices.Identity.Application.Commands.Register;

/// <summary>
/// Represents the register command handler class.
/// </summary>
/// <param name="logger">The logger.</param>
/// <param name="userManager">The user manager.</param>
/// <param name="signInManager">The sign in manager.</param>
/// <param name="jwtOptions">The json web token options.</param>
internal sealed class RegisterCommandHandler(
        ILogger<RegisterCommandHandler> logger,
        UserManager<User> userManager,
        SignInManager<User> signInManager,
        IOptions<JwtOptions> jwtOptions)
    : ICommandHandler<RegisterCommand, LoginResponse<Result<User>>>
{
    private readonly JwtOptions _jwtOptions = jwtOptions.Value;
    private readonly SignInManager<User> _signInManager = signInManager ?? throw new ArgumentNullException();
 
    /// <inheritdoc />
    public async Task<LoginResponse<Result<User>>> Handle(
        RegisterCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            logger.LogInformation($"Request for login an account - {request.UserName} {request.LastName}");
            
            Result<FirstName> firstNameResult = FirstName.Create(request.FirstName);
            Result<LastName> lastNameResult = LastName.Create(request.LastName);
            Result<EmailAddress> emailResult = EmailAddress.Create(request.Email);
            Result<Password> passwordResult = Password.Create(request.Password);
            
            var user = await userManager.FindByNameAsync(request.UserName);

            if (user is not null)
            {
                logger.LogWarning("User with the same name already taken");
                throw new NotFoundException(nameof(user), "User with the same name");
            }

            user = User.Create(firstNameResult.Value, lastNameResult.Value,request.UserName, emailResult.Value, passwordResult.Value);
            
            var result = await userManager.CreateAsync(user, request.Password);

            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, false);
                
                logger.LogInformation($"User authorized - {user.UserName} {DateTime.UtcNow}");
            }
            
            var (refreshToken, refreshTokenExpireAt) = user.GenerateRefreshToken(_jwtOptions);
            
            if (result.Succeeded)
            {
                user.RefreshToken = refreshToken;
            }
            
            return new LoginResponse<Result<User>>
            {
                Description = "Register account",
                StatusCode = HttpStatusCode.OK,
                Data = System.Threading.Tasks.Task.FromResult(Result.Create(user, DomainErrors.General.ServerError)),
                AccessToken = user.GenerateAccessToken(_jwtOptions),
                RefreshToken = refreshToken,
                RefreshTokenExpireAt = refreshTokenExpireAt
            };
        }
        catch (Exception exception)
        {
            logger.LogError(exception, $"[RegisterCommandHandler]: {exception.Message}");
            throw new AuthenticationException(exception.Message);
        }
    }
}