using Tekoding.KoIdentity.Abstraction.Models;

namespace Tekoding.KoIdentity.Core.Models;

/// <summary>
/// Provides the default implementation of an user.
/// </summary>
public class User : Entity
{
#nullable disable

    /// <summary>
    /// The username of the current <see cref="User"/> object.
    /// </summary>
    public string Username { get; set; }

    /// <summary>
    /// The password of the current <see cref="User"/> object.
    /// </summary>
    public string Password { get; set; }

#nullable restore
}