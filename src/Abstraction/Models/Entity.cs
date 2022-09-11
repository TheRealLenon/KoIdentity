namespace Tekoding.KoIdentity.Abstraction.Models;

/// <summary>
/// Provides an abstraction of entities.
/// </summary>
public abstract class Entity
{
#nullable disable
    /// <summary>
    /// The unique identifier (id) of the current <see cref="Entity"/>.
    /// </summary>
    public Guid Id { get; }

    /// <summary>
    /// The date, when the current <see cref="Entity"/> was created.
    /// </summary>
    public DateTime CreationDate { get; internal set; }

    /// <summary>
    /// The date, when the current <see cref="Entity"/> was changed.
    /// </summary>
    public DateTime ChangeDate { get; internal set; }
#nullable restore
}