using FluentValidation.Results;
using Tekoding.KoIdentity.Abstraction.Errors;

namespace Tekoding.KoIdentity.Abstraction.Extensions;

/// <summary>
/// Extension methods for <see cref="ValidationResult"/>s used within KoIdentity.
/// </summary>
public static class ValidationResultExtension
{
    /// <summary>
    /// Transforms the <see cref="ValidationResult.Errors"/> into an <see cref="Array"/> of
    /// <see cref="Error"/>s used within KoIdentity.
    /// </summary>
    /// <param name="validationResult">The <see cref="ValidationResult"/> containing the errors.</param>
    /// <returns>Returns an <see cref="Array"/> of <see cref="Error"/>s.</returns>
    public static Error[] TransformValidationFailuresToErrors(this ValidationResult validationResult)
    {
        return validationResult.Errors.Select(failure => new Error
        {
            Code = failure.ErrorCode ?? failure.PropertyName,
            Description = failure.ErrorMessage
        }).ToArray();
    } 
}