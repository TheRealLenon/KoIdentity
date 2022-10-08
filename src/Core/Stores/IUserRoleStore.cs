using Tekoding.KoIdentity.Abstraction.Errors;
using Tekoding.KoIdentity.Abstraction.Stores;
using Tekoding.KoIdentity.Core.Models;

namespace Tekoding.KoIdentity.Core.Stores;

/// <summary>
/// Represents a persistence store abstraction API for <see cref="UserRole"/>s.
/// </summary>
public interface IUserRoleStore : IEntityStore<UserRole<User>>
{
    /// <summary>
    /// Gets all <see cref="Models.Role"/>s from the backing store, if any, currently assigned to the
    /// <see cref="Models.User"/> as an asynchronous operation.
    /// </summary>
    /// <param name="userId">The unique identifier of the <see cref="Models.User"/> whose <see cref="Models.Role"/>s
    /// should be retrieved.</param>
    /// <param name="cancellationToken">
    /// The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.
    /// </param>
    /// <returns>
    /// The <see cref="Task{TResult}"/> that represents the asynchronous operation, containing the
    /// <see cref="OperationResult"/> including a list of all existing <see cref="Models.Role"/>s, if any.
    /// </returns>
    public Task<OperationResult> GetRolesByUserId(Guid userId, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Gets all <see cref="Models.Role"/>s from the backing store, if any, currently assigned to the
    /// <see cref="Models.User"/> as an asynchronous operation.
    /// </summary>
    /// <param name="roleId">The unique identifier of the <see cref="Models.Role"/> whose <see cref="Models.User"/>s
    /// should be retrieved.</param>
    /// <param name="cancellationToken">
    /// The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.
    /// </param>
    /// <returns>
    /// The <see cref="Task{TResult}"/> that represents the asynchronous operation, containing the
    /// <see cref="OperationResult"/> including a list of all existing <see cref="Models.User"/>s, if any.
    /// </returns>
    public Task<OperationResult> GetUsersByRoleId(Guid roleId, CancellationToken cancellationToken = default);
}