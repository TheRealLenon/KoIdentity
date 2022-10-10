using FluentValidation;
using Tekoding.KoIdentity.Abstraction.Validations;
using Tekoding.KoIdentity.Core.Models;

namespace Tekoding.KoIdentity.Core.Validations;

/// <summary>
/// Defines a set of validation rules for a particular object for <see cref="User"/>s to be validated.
/// </summary>
public class UserValidator<TUser> : EntityValidator<TUser>
    where TUser : User
{
    /// <summary>
    /// Creates a new instance of the <see cref="UserValidator{TUser}"/>.
    /// </summary>
    public UserValidator()
    {
        RuleFor(u => u.Username)
            .NotEmpty().WithErrorCode("EmptyUsername").WithMessage("Username is Empty");
        RuleFor(u => u.Password)
            .NotEmpty().WithErrorCode("EmptyUsername").WithMessage("Password is Empty");
    }
}