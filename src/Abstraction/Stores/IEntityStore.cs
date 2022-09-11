using Tekoding.KoIdentity.Abstraction.Errors;
using Tekoding.KoIdentity.Abstraction.Models;

namespace Tekoding.KoIdentity.Abstraction.Stores;

/// <summary>
/// Represents a persistence store abstraction API for <typeparamref name="TEntity"/>s.
/// </summary>
/// <typeparam name="TEntity">The type encapsulating an <see cref="Entity"/>.</typeparam>
public interface IEntityStore<in TEntity> where TEntity : Entity
{
    /// <summary>
    /// Creates the specified <typeparamref name="TEntity"/> in the backing store, as an asynchronous operation.
    /// </summary>
    /// <param name="entity">The <typeparamref name="TEntity"/> to create.</param>
    /// <param name="cancellationToken">
    /// The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.
    /// </param>
    /// <returns>
    /// The <see cref="Task{TResult}"/> that represents the asynchronous operation, containing the
    /// <see cref="OperationResult"/> indicating, if the creation succeeded or not.
    /// </returns>
    public Task<OperationResult> CreateAsync(TEntity entity, CancellationToken cancellationToken = default);
}