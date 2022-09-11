using FluentValidation;
using Tekoding.KoIdentity.Abstraction.Validations;

namespace Tekoding.KoIdentity.Abstraction.Test.Helper;

internal class BaseEntityValidator: EntityValidator<BaseEntity>
{
    internal BaseEntityValidator()
    {
        RuleFor(e => e.TestProp).NotEmpty()
            .WithErrorCode("EmptyHelperProp")
            .WithMessage("Property is empty");
    }
}