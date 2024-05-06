using System.Net;
using Microsoft.AspNetCore.Identity;
using ToDoListMicroservices.Application.ApiHelpers.Responses;
using ToDoListMicroservices.Application.Core.Abstractions.Helpers.JWT;
using ToDoListMicroservices.Application.Core.Abstractions.Messaging;
using ToDoListMicroservices.Database.Common.Abstractions;
using ToDoListMicroservices.Domain.Common.Core.Errors;
using ToDoListMicroservices.Domain.Common.Core.Primitives.Maybe;
using ToDoListMicroservices.Domain.Common.Core.Primitives.Result;
using ToDoListMicroservices.Domain.Common.ValueObjects;
using ToDoListMicroservices.Identity.Domain.Entities;

namespace ToDoListMicroservices.Identity.Application.Commands.ChangeName;

/// <summary>
/// Represents the <see cref="ChangeNameCommand"/> handler.
/// </summary>
internal sealed class ChangeNameCommandHandler : ICommandHandler<ChangeNameCommand, IBaseResponse<Result<User>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly UserManager<User> _userManager;
    private readonly IUserIdentifierProvider _userIdentifier;

    /// <summary>
    /// Initializes a new instance of the <see cref="ChangeNameCommandHandler"/> class.
    /// </summary>
    /// <param name="unitOfWork">The unit of work.</param>
    /// <param name="userManager">The user manager.</param>
    /// <param name="userIdentifier">The user identifier provider.</param>
    public ChangeNameCommandHandler(
        IUnitOfWork unitOfWork,
        UserManager<User> userManager,
        IUserIdentifierProvider userIdentifier)
    {
        _unitOfWork = unitOfWork;
        _userManager = userManager;
        _userIdentifier = userIdentifier;
    }

    /// <inheritdoc />
    public async Task<IBaseResponse<Result<User>>> Handle(ChangeNameCommand request, CancellationToken cancellationToken)
    {
        Result<FirstName> nameResult = FirstName.Create(request.FirstName);

        if (nameResult.IsFailure)
        {
            return new BaseResponse<Result<User>>
            {
                Data = Result.Failure<User>(nameResult.Error),
                StatusCode = HttpStatusCode.InternalServerError,
                Description = "First Name result is failure."
            };
        }
        
        Result<LastName> lastNameResult = LastName.Create(request.LastName);

        if (lastNameResult.IsFailure)
        {
            return new BaseResponse<Result<User>>
            {
                Data = Result.Failure<User>(lastNameResult.Error),
                StatusCode = HttpStatusCode.InternalServerError,
                Description = "Last Name result is failure."
            };
        }
        
        Maybe<User> maybeUser = await _userManager.FindByIdAsync(_userIdentifier.UserId.ToString()) 
                                ?? throw new ArgumentException();

        if (maybeUser.HasNoValue)
        {
            return new BaseResponse<Result<User>>
            {
                Data = Result.Failure<User>(DomainErrors.User.NotFound),
                StatusCode = HttpStatusCode.NotFound,
                Description = "User not found."
            };
        }

        User user = maybeUser.Value;

        user.ChangeName(request.FirstName,request.LastName);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new BaseResponse<Result<User>>
        {
            Data = Result.Create(user, DomainErrors.General.ServerError),
            Description = "Change name.",
            StatusCode = HttpStatusCode.OK
        };
    }
}