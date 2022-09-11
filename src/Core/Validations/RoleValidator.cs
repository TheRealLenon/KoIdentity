using FluentValidation;
using Tekoding.KoIdentity.Abstraction.Validations;
using Tekoding.KoIdentity.Core.Models;

namespace Tekoding.KoIdentity.Core.Validations;

/// <summary>
/// Defines a set of validation rules for a particular object for <see cref="Role"/>s to be validated.
/// </summary>
public class RoleValidator : EntityValidator<Role>
{
    /// <summary>
    /// Creates a new instance of the <see cref="RoleValidator"/>.
    /// </summary>
    public RoleValidator()
    {
        RuleFor(r => r.Name)
            .NotEmpty().WithErrorCode("EmptyUsername").WithMessage("Username is Empty");
    }
}