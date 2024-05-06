using FluentValidation;
using ToDoListMicroservices.Application.Core.Errors;
using ToDoListMicroservices.Application.Core.Extensions;

namespace ToDoListMicroservices.Identity.Application.Commands.ChangePassword;

/// <summary>
/// Represents the <see cref="ChangePasswordCommand"/> validator.
/// </summary>
public sealed class ChangePasswordCommandValidator : AbstractValidator<ChangePasswordCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ChangePasswordCommandValidator"/> class.
    /// </summary>
    public ChangePasswordCommandValidator()
    {
        RuleFor(x => x.Password)
            .NotEmpty()
            .WithError(ValidationErrors.ChangePassword.PasswordIsRequired);
    }
}