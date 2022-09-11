namespace Tekoding.KoIdentity.Core.Models.Dtos;

/// <summary>
/// Provides a data transfer object for creating an user.
/// </summary>
public class UserCreationDto
{
#nullable disable
    /// <summary>
    /// The username of the user to create.
    /// </summary>
    /// <example>IronMan</example>
    public string Username { get; init; }
    
    /// <summary>
    /// The password of the user to create.
    /// </summary>
    /// <example>HulkIsStrongerThenMe</example>
    public string Password { get; init; }
#nullable restore
}