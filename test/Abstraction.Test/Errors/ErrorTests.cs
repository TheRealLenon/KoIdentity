using FluentAssertions;
using NUnit.Framework;
using Tekoding.KoIdentity.Abstraction.Errors;

namespace Tekoding.KoIdentity.Abstraction.Test.Errors;

/// <summary>
/// Provides unit tests for testing the <see cref="Error"/> model.
/// </summary>
public class ErrorTests
{
    /// <summary>
    /// Testing the creation of the <see cref="Error"/> model and the assignment of the values.
    /// </summary>
    [Test]
    public void Error_ValidateCreation()
    {
        var error = new Error
        {
            Code = "ExampleErrorCode",
            Description = "ExampleErrorDescription"
        };

        error.Code.Should().Be("ExampleErrorCode");
        error.Description.Should().Be("ExampleErrorDescription");
    }
}