using Microsoft.EntityFrameworkCore;
using Tekoding.KoIdentity.Abstraction.Stores;
using Tekoding.KoIdentity.Core.Models;
using Tekoding.KoIdentity.Core.Validations;

namespace Tekoding.KoIdentity.Core.Stores;

/// <summary>
/// Provides the default implementation of the <see cref="IEntityStore{TEntity}"/> using <see cref="User"/> as entity.
/// </summary>
public class UserStore : EntityStore<User>, IUserStore
{
    /// <summary>
    /// Creates a new instance of the <see cref="UserStore"/>.
    /// </summary>
    /// <param name="dbContext">The <see cref="DbContext"/> used to access the store.</param>
    /// <param name="userValidator">The <see cref="UserValidator"/> used for validating an <see cref="User"/>.</param>
    public UserStore(DbContext dbContext, UserValidator userValidator) : base(dbContext, userValidator)
    {
    }
}