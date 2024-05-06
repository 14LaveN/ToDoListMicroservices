using FluentValidation;
using ToDoListMicroservices.Application.Core.Errors;
using ToDoListMicroservices.Application.Core.Extensions;

namespace ToDoListMicroservices.Identity.Application.Commands.ChangeName;

/// <summary>
/// Represents the <see cref="ChangeNameCommand"/> validator class.
/// </summary>
public sealed class ChangeNameCommandValidator
    : AbstractValidator<ChangeNameCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ChangeNameCommandValidator"/> class.
    /// </summary>
    public ChangeNameCommandValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty()
            .WithError(ValidationErrors.ChangeName.NameIsRequired);
        
        RuleFor(x => x.LastName)
            .NotEmpty()
            .WithError(ValidationErrors.ChangeName.NameIsRequired);
    }
}