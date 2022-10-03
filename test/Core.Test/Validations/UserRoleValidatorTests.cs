using FluentAssertions;
using NUnit.Framework;
using Tekoding.KoIdentity.Core.Models;
using Tekoding.KoIdentity.Core.Validations;

namespace Tekoding.KoIdentity.Core.Test.Validations;

/// <summary>
/// Provides a set of unit tests that are performed for testing the <see cref="UserRoleValidator"/>
/// </summary>
public class UserRoleValidatorTests
{
    /// <summary>
    /// Checking the <see cref="UserRoleValidator"/> to successfully validate an <see cref="UserRole"/> model.
    /// </summary>
    [Test]
    public void UserRoleValidator_SuccessfulValidation()
    {
        var userValidator = new UserRoleValidator();
        
        UserRole userToValidate = new()
        {
            UserId = Guid.NewGuid(),
            RoleId = Guid.NewGuid()
        };

        var validationResult = userValidator.Validate(userToValidate);

        validationResult.IsValid.Should().BeTrue();
        validationResult.Errors.Should().BeNullOrEmpty();
    }
}