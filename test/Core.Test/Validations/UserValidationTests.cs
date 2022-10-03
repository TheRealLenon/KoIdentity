using FluentAssertions;
using NUnit.Framework;
using Tekoding.KoIdentity.Core.Models;
using Tekoding.KoIdentity.Core.Validations;

namespace Tekoding.KoIdentity.Core.Test.Validations;

/// <summary>
/// Provides a set of unit tests that are performed for testing the <see cref="UserValidator"/>
/// </summary>
public class UserValidationTests
{
    /// <summary>
    /// Checking the <see cref="UserValidator"/> to successfully validate an <see cref="User"/> model.
    /// </summary>
    [Test]
    public void UserValidator_SuccessfulValidation()
    {
        var userValidator = new UserValidator();
        User userToValidate = new()
        {
            Username = "SuperDuperUsername",
            Password = "SuperDuperPassword"
        };

        var validationResult = userValidator.Validate(userToValidate);

        validationResult.IsValid.Should().BeTrue();
        validationResult.Errors.Should().BeNullOrEmpty();
    }
}