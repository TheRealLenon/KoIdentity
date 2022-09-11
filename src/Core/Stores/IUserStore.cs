using Tekoding.KoIdentity.Abstraction.Stores;
using Tekoding.KoIdentity.Core.Models;

namespace Tekoding.KoIdentity.Core.Stores;

/// <summary>
/// Represents a persistence store abstraction API for <see cref="User"/>s.
/// </summary>
public interface IUserStore: IEntityStore<User>
{
    
}