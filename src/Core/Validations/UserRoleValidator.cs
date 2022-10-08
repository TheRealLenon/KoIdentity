using FluentValidation;
using Tekoding.KoIdentity.Abstraction.Validations;
using Tekoding.KoIdentity.Core.Models;

namespace Tekoding.KoIdentity.Core.Validations;

/// <summary>
/// Defines a set of validation rules for a particular object for <see cref="UserRole"/>s to be validated.
/// </summary>
public class UserRoleValidator : EntityValidator<UserRole<User>>
{
    /// <summary>
    /// Creates a new instance of the <see cref="UserRoleValidator"/>.
    /// </summary>
    public UserRoleValidator()
    {
        RuleFor(ur => ur.RoleId)
            .NotEmpty().WithErrorCode("EmptyRoleId").WithMessage("Role-Id is Empty");
        RuleFor(ur => ur.UserId)
            .NotEmpty().WithErrorCode("EmptyUserId").WithMessage("User-Id is Empty");
    }
}