using FluentValidation;
using FluentValidation.Results;
using Tekoding.KoIdentity.Abstraction.Errors;
using Tekoding.KoIdentity.Abstraction.Models;

namespace Tekoding.KoIdentity.Abstraction.Validations;

/// <summary>
/// Defines a set of validation rules for a particular object where <typeparamref name="TEntity"/> is the type of class
/// to be validated.
/// </summary>
/// <typeparam name="TEntity">The type encapsulating an <see cref="Entity"/>.</typeparam>
public abstract class EntityValidator<TEntity> : AbstractValidator<TEntity>
    where TEntity : Entity
{
    /// <inheritdoc />
    protected override bool PreValidate(ValidationContext<TEntity> context, ValidationResult result)
    {
        if (context.InstanceToValidate != null)
        {
            return base.PreValidate(context, result);
        }

        var objectNullFailure = ErrorDescriber.ObjectNullFailure();
        result.Errors.Add(new ValidationFailure(objectNullFailure.Code, objectNullFailure.Description));

        return false;
    }
}