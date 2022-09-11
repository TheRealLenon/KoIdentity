using System.Linq;
using FluentAssertions;
using FluentValidation.Results;
using NUnit.Framework;
using Tekoding.KoIdentity.Abstraction.Errors;
using Tekoding.KoIdentity.Abstraction.Extensions;
using Tekoding.KoIdentity.Abstraction.Test.Helper;

namespace Tekoding.KoIdentity.Abstraction.Test.Validations;

/// <summary>
/// Provides a set of unit tests that are performed for testing the extension methods used on the
/// <see cref="ValidationResult"/>.
/// </summary>
public class ValidationResultExtensionTests
{
    /// <summary>
    /// Testing the transformation for an <b>failed</b> <see cref="ValidationResult"/> into an array of
    /// <see cref="Error"/>s.
    /// </summary>
    [Test]
    public void ValidationResultExtension_ValidateTransformationOnFailedValidation()
    {
        var entityValidator = new BaseEntityValidator();
        BaseEntity? entityToValidate = null;
        var objectNullFailureError = ErrorDescriber.ObjectNullFailure();

#pragma warning disable CS8604
        var validationResult = entityValidator.Validate(entityToValidate);
#pragma warning restore CS8604
        var validationErrors = validationResult.TransformValidationFailuresToErrors();

        validationResult.IsValid.Should().BeFalse();
        validationErrors.Should().NotBeNull().And.BeOfType<Error[]>().And.HaveCount(1);
        validationErrors.First().Code.Should().Be(objectNullFailureError.Code);
        validationErrors.First().Description.Should().Be(objectNullFailureError.Description);
    }

    /// <summary>
    /// Testing the transformation for an <b>successful</b> <see cref="ValidationResult"/> without containing any
    /// <see cref="Error"/>s.
    /// </summary>
    [Test]
    public void ValidationResultExtension_ValidateTransformationOnSucceededValidation()
    {
        var entityValidator = new BaseEntityValidator();
        BaseEntity entityToValidate = new();

        var validationResult = entityValidator.Validate(entityToValidate);
        validationResult.IsValid.Should().BeTrue();
        validationResult.Errors.Should().BeNullOrEmpty();
    }
}