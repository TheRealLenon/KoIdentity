using FluentAssertions;
using NUnit.Framework;
using Tekoding.KoIdentity.Abstraction.Errors;

namespace Tekoding.KoIdentity.Abstraction.Test.Errors;

/// <summary>
/// Defines a set of unit tests for the <see cref="ErrorDescriber"/> describing <see cref="Error"/>s.
/// </summary>
public class ErrorDescriberTests
{
    /// <summary>
    /// Checking the <see cref="ErrorDescriber"/> to return an <b>ObjectNullFailure</b> with the correct code.
    /// </summary>
    [Test]
    public void ErrorDescriber_ValidateObjectNullFailure()
    {
        var error = ErrorDescriber.ObjectNullFailure();

        error.Code.Should().NotBeNullOrWhiteSpace().And.Be(nameof(ErrorDescriber.ObjectNullFailure));
    }
    
    /// <summary>
    /// Checking the <see cref="ErrorDescriber"/> to return an <b>ObjectInvalidFailure</b> with the correct code.
    /// </summary>
    [Test]
    public void ErrorDescriber_ValidateInvalidFailure()
    {
        var error = ErrorDescriber.ObjectInvalidFailure();

        error.Code.Should().NotBeNullOrWhiteSpace().And.Be(nameof(ErrorDescriber.ObjectInvalidFailure));
    }
    
    /// <summary>
    /// Checking the <see cref="ErrorDescriber"/> to return an <b>ObjectStateUnmodifiedFailure</b> with the correct code.
    /// </summary>
    [Test]
    public void ErrorDescriber_ValidateObjectStateUnmodifiedFailure()
    {
        var error = ErrorDescriber.ObjectStateUnmodifiedFailure();

        error.Code.Should().NotBeNullOrWhiteSpace().And.Be(nameof(ErrorDescriber.ObjectStateUnmodifiedFailure));
    }
    
    /// <summary>
    /// Checking the <see cref="ErrorDescriber"/> to return an <b>DatabaseCreationFailure</b> with the correct code.
    /// </summary>
    [Test]
    public void ErrorDescriber_ValidateDatabaseCreationFailure()
    {
        var error = ErrorDescriber.DatabaseCreationFailure();

        error.Code.Should().NotBeNullOrWhiteSpace().And.Be(nameof(ErrorDescriber.DatabaseCreationFailure));
    }
    
    /// <summary>
    /// Checking the <see cref="ErrorDescriber"/> to return an <b>DatabaseUpdateFailure</b> with the correct code.
    /// </summary>
    [Test]
    public void ErrorDescriber_ValidateDatabaseUpdateFailure()
    {
        var error = ErrorDescriber.DatabaseUpdateFailure();

        error.Code.Should().NotBeNullOrWhiteSpace().And.Be(nameof(ErrorDescriber.DatabaseUpdateFailure));
    }
    
    /// <summary>
    /// Checking the <see cref="ErrorDescriber"/> to return an <b>DatabaseDeletionFailure</b> with the correct code.
    /// </summary>
    [Test]
    public void ErrorDescriber_ValidateDatabaseDeletionFailure()
    {
        var error = ErrorDescriber.DatabaseDeletionFailure();

        error.Code.Should().NotBeNullOrWhiteSpace().And.Be(nameof(ErrorDescriber.DatabaseDeletionFailure));
    }
    
    /// <summary>
    /// Checking the <see cref="ErrorDescriber"/> to return an <b>DatabaseSelectionFailure</b> with the correct code.
    /// </summary>
    [Test]
    public void ErrorDescriber_ValidateDatabaseSelectionFailure()
    {
        var error = ErrorDescriber.DatabaseSelectionFailure();

        error.Code.Should().NotBeNullOrWhiteSpace().And.Be(nameof(ErrorDescriber.DatabaseSelectionFailure));
    }
}