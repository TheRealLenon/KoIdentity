using Tekoding.KoIdentity.Abstraction.Models;

namespace Tekoding.KoIdentity.Core.Models;

/// <summary>
/// Provides the default implementation of a role.
/// </summary>
public class Role : Entity
{
#nullable disable

    /// <summary>
    /// The name of the current <see cref="Role"/>.
    /// </summary>
    public string Name { get; set; }

#nullable restore
}