using Microsoft.EntityFrameworkCore;
using Tekoding.KoIdentity.Abstraction.Errors;
using Tekoding.KoIdentity.Abstraction.Stores;
using Tekoding.KoIdentity.Core.Models;
using Tekoding.KoIdentity.Core.Validations;

namespace Tekoding.KoIdentity.Core.Stores;

/// <summary>
/// Provides the default implementation of the <see cref="IEntityStore{TEntity}"/> using <see cref="UserRole"/> as
/// the entity.
/// </summary>
public class UserRoleStore : EntityStore<UserRole>, IUserRoleStore
{
    private DbContext DbContext { get; }

    /// <summary>
    /// Creates a new instance of the <see cref="UserStore"/>.
    /// </summary>
    /// <param name="dbContext">The <see cref="DbContext"/> used to access the store.</param>
    /// <param name="userRoleValidator">The <see cref="UserRoleValidator"/> used for validating an <see cref="UserRole"/>.</param>
    public UserRoleStore(DbContext dbContext, UserRoleValidator userRoleValidator) : base(dbContext, userRoleValidator)
    {
        DbContext = dbContext;
    }

    /// <inheritdoc />
    public async Task<OperationResult> GetRolesByUserId(Guid userId, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (userId == Guid.Empty)
        {
            return OperationResult.Failed(ErrorDescriber.ObjectInvalidFailure());
        }

        try
        {
            return OperationResult.SuccessWithPayload(
                await DbContext.Set<UserRole>().Include(ur => ur.Role)
                    .Where(ur => ur.UserId == userId).Select(ur => ur.Role)
                    .ToListAsync(cancellationToken)
                );
        }
        catch (Exception)
        {
            return OperationResult.Failed(ErrorDescriber.DatabaseSelectionFailure());
        }
    }

    /// <inheritdoc />
    public async Task<OperationResult> GetUsersByRoleId(Guid roleId, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (roleId == Guid.Empty)
        {
            return OperationResult.Failed(ErrorDescriber.ObjectInvalidFailure());
        }

        try
        {
            return OperationResult.SuccessWithPayload(
                await DbContext.Set<UserRole>().Include(ur => ur.Role)
                    .Where(ur => ur.RoleId == roleId).Select(ur => ur.User)
                    .ToListAsync(cancellationToken)
            );
        }
        catch (Exception)
        {
            return OperationResult.Failed(ErrorDescriber.DatabaseSelectionFailure());
        }
    }
}