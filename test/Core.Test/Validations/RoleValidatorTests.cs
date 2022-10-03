using FluentAssertions;
using NUnit.Framework;
using Tekoding.KoIdentity.Core.Models;
using Tekoding.KoIdentity.Core.Validations;

namespace Tekoding.KoIdentity.Core.Test.Validations;

/// <summary>
/// Provides a set of unit tests that are performed for testing the <see cref="RoleValidator"/>
/// </summary>
public class RoleValidatorTests
{
    /// <summary>
    /// Checking the <see cref="RoleValidator"/> to successfully validate an <see cref="Role"/> model.
    /// </summary>
    [Test]
    public void RoleValidator_SuccessfulValidation()
    {
        var userValidator = new RoleValidator();
        Role userToValidate = new()
        {
            Name = "SuperDuperRoleName"
        };

        var validationResult = userValidator.Validate(userToValidate);

        validationResult.IsValid.Should().BeTrue();
        validationResult.Errors.Should().BeNullOrEmpty();
    }
}