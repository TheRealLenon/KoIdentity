using Tekoding.KoIdentity.Abstraction.Resources;

namespace Tekoding.KoIdentity.Abstraction.Errors;

/// <summary>
/// Provides a service to use localization while facing errors.
/// </summary>
/// <remarks>
/// This errors are returned to controllers or services and are generally used to display messages to end users.
/// </remarks>
public static class ErrorDescriber
{
    /// <summary>
    /// Describes an <see cref="Error"/> indicating an <b>ObjectNullFailure</b>
    /// </summary>
    /// <returns>
    /// Returns an <see cref="Error"/> indicating that an operation failed because the object does not point into
    /// memory.
    /// </returns>
    public static Error ObjectNullFailure() => new()
    {
        Code = nameof(ObjectNullFailure),
        Description = ErrorResources.ObjectNullFailure
    };

    /// <summary>
    /// Describes an <see cref="Error"/> indicating an <b>DatabaseCreationFailure</b>
    /// </summary>
    /// <returns>
    /// Returns an <see cref="Error"/> indicating that an operation failed because the object could not be created in
    /// the database.
    /// </returns>
    public static Error DatabaseCreationFailure() => new()
    {
        Code = nameof(DatabaseCreationFailure),
        Description = ErrorResources.DatabaseCreationFailure
    };
}