using Tekoding.KoIdentity.Abstraction.Models;

namespace Tekoding.KoIdentity.Core.Models;

/// <summary>
/// Provides the default implementation of user roles.
/// </summary>
public class UserRole<TUser> : Entity
    where TUser : User
{
#nullable disable
    /// <summary>
    /// The unique identifier of the <see cref="User"/> this <see cref="UserRole"/> belongs to.
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// The <see cref="Models.User"/> this <see cref="UserRole"/> belongs to.
    /// </summary>
    public TUser User { get; set; }

    /// <summary>
    /// The unique identifier of the <see cref="Role"/> this <see cref="UserRole"/> belongs to.
    /// </summary>
    public Guid RoleId { get; set; }

    /// <summary>
    /// The <see cref="Models.Role"/> this <see cref="UserRole"/> belongs to.
    /// </summary>
    public Role Role { get; set; }
#nullable enable
}