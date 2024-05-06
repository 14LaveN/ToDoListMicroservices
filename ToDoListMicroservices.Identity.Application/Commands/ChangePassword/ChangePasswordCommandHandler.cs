using Microsoft.AspNetCore.Identity;
using ToDoListMicroservices.Application.Core.Abstractions.Helpers.JWT;
using ToDoListMicroservices.Application.Core.Abstractions.Messaging;
using ToDoListMicroservices.Domain.Common.Core.Errors;
using ToDoListMicroservices.Domain.Common.Core.Primitives.Maybe;
using ToDoListMicroservices.Domain.Common.Core.Primitives.Result;
using ToDoListMicroservices.Domain.Common.ValueObjects;
using ToDoListMicroservices.Identity.Database.Data.Interfaces;
using ToDoListMicroservices.Identity.Domain.Entities;

namespace ToDoListMicroservices.Identity.Application.Commands.ChangePassword;

/// <summary>
/// Represents the <see cref="ChangePasswordCommand"/> handler.
/// </summary>
internal sealed class ChangePasswordCommandHandler : ICommandHandler<ChangePasswordCommand, Result<User>>
{
    private readonly IUserUnitOfWork _unitOfWork;
    private readonly UserManager<User> _userManager;
    private readonly IUserIdentifierProvider _userIdentifier;

    /// <summary>
    /// Initializes a new instance of the <see cref="ChangePasswordCommandHandler"/> class.
    /// </summary>
    /// <param name="unitOfWork">The unit of work.</param>
    /// <param name="userManager">The user manager.</param>
    /// <param name="userIdentifier">The user identifier provider.</param>
    public ChangePasswordCommandHandler(
        IUserUnitOfWork unitOfWork,
        UserManager<User> userManager,
        IUserIdentifierProvider userIdentifier)
    {
        _unitOfWork = unitOfWork;
        _userManager = userManager;
        _userIdentifier = userIdentifier;
    }

    /// <inheritdoc />
    public async Task<Result<User>> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
    {
        Result<Password> passwordResult = Password.Create(request.Password);

        if (passwordResult.IsFailure)
        {
            return Result.Failure<User>(passwordResult.Error);
        }

        Maybe<User> maybeUser = await _userManager.FindByIdAsync(_userIdentifier.UserId.ToString()) 
                                ?? throw new ArgumentException();

        if (maybeUser.HasNoValue)
        {
            return  Result.Failure<User>(DomainErrors.User.NotFound);
        }

        User user = maybeUser.Value;

        var passwordHash = _userManager.PasswordHasher.HashPassword(user, passwordResult.Value);

        Result result = user.ChangePassword(passwordHash);

        if (result.IsFailure)
        {
            return Result.Failure<User>(result.Error);
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Create(user, DomainErrors.General.ServerError);
    }
}