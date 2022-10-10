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
    /// The unique identifier of the <see cref="User"/> this <see cref="UserRole{TUser}"/> belongs to.
    /// </summary>
    public Guid UserId { get; init; }

    /// <summary>
    /// The <see cref="Models.User"/> this <see cref="UserRole{TUser}"/> belongs to.
    /// </summary>
    // ReSharper disable once UnusedAutoPropertyAccessor.Local
    public TUser User { get; private set; }

    /// <summary>
    /// The unique identifier of the <see cref="Role"/> this <see cref="UserRole{TUser}"/> belongs to.
    /// </summary>
    public Guid RoleId { get;  init; }

    /// <summary>
    /// The <see cref="Models.Role"/> this <see cref="UserRole{TUser}"/> belongs to.
    /// </summary>
    // ReSharper disable once UnusedAutoPropertyAccessor.Local
    public Role Role { get; private set; }
#nullable enable
}