namespace Tekoding.KoIdentity.Abstraction.Errors;

/// <summary>
/// Encapsulates an error used by KoIdentity.
/// </summary>
public struct Error
{
    /// <summary>
    /// Gets or initializes the code for this error.
    /// </summary>
    public string Code { get; init; }
    
    /// <summary>
    /// Gets or initializes the description for this error.
    /// </summary>
    public string Description { get; init; }
}