using Microsoft.EntityFrameworkCore;
using Tekoding.KoIdentity.Abstraction.Stores;
using Tekoding.KoIdentity.Core.Models;
using Tekoding.KoIdentity.Core.Validations;

namespace Tekoding.KoIdentity.Core.Stores;

/// <summary>
/// Provides the default implementation of the <see cref="IEntityStore{TEntity}"/> using <see cref="Role"/> as entity.
/// </summary>
public class RoleStore: EntityStore<Role>, IRoleStore
{
    /// <summary>
    /// Creates a new instance of the <see cref="UserStore"/>.
    /// </summary>
    /// <param name="dbContext">The <see cref="DbContext"/> used to access the store.</param>
    /// <param name="roleValidator">The <see cref="RoleValidator"/> used for validating an <see cref="Role"/>.</param>
    public RoleStore(DbContext dbContext, RoleValidator roleValidator) : base(dbContext, roleValidator)
    {
    }
}