using Microsoft.EntityFrameworkCore;
using Tekoding.KoIdentity.Abstraction.Errors;
using Tekoding.KoIdentity.Abstraction.Extensions;
using Tekoding.KoIdentity.Abstraction.Models;
using Tekoding.KoIdentity.Abstraction.Validations;

namespace Tekoding.KoIdentity.Abstraction.Stores;

/// <summary>
/// Represents the abstracted store implementation for <typeparamref name="TEntity"/>s.
/// </summary>
/// <typeparam name="TEntity">The type encapsulating an <see cref="Entity"/></typeparam>
public abstract class EntityStore<TEntity> : IEntityStore<TEntity>
    where TEntity : Entity
{
    private DbContext DbContext { get; }
    private EntityValidator<TEntity> EntityValidator { get; }

    /// <summary>
    /// Creates a new instance of the <see cref="EntityStore{TEntity}"/>.
    /// </summary>
    /// <param name="dbContext">
    /// The <see cref="Microsoft.EntityFrameworkCore.DbContext"/> used to access the store.
    /// </param>
    /// <param name="entityValidator">The <see cref="EntityValidator{TEntity}"/> used to validate the <typeparamref name="TEntity"/>.</param>
    protected EntityStore(DbContext dbContext, EntityValidator<TEntity> entityValidator)
    {
        DbContext = dbContext;
        EntityValidator = entityValidator;
    }

    /// <inheritdoc />
    public async Task<OperationResult> CreateAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var entityValidationResult = await EntityValidator.ValidateAsync(entity, cancellationToken);

        if (!entityValidationResult.IsValid)
        {
            return OperationResult.Failed(entityValidationResult.TransformValidationFailuresToErrors());
        }

        await DbContext.AddAsync(entity, cancellationToken);

        try
        {
            await DbContext.SaveChangesAsync(cancellationToken);
        }
        catch
        {
            return OperationResult.Failed(ErrorDescriber.DatabaseCreationFailure());
        }
        
        return OperationResult.Success;
    }
    
    /// <inheritdoc />
    public async Task<OperationResult> DeleteAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var entityValidationResult = await EntityValidator.ValidateAsync(entity, cancellationToken);

        if (!entityValidationResult.IsValid)
        {
            return OperationResult.Failed(entityValidationResult.TransformValidationFailuresToErrors());
        }

        DbContext.Remove(entity);

        try
        {
            await DbContext.SaveChangesAsync(cancellationToken);
        }
        catch (Exception)
        {
            return OperationResult.Failed(ErrorDescriber.DatabaseDeletionFailure());
        }

        return OperationResult.Success;
    }
    
    /// <inheritdoc />
    public async Task<OperationResult> FindByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (id == Guid.Empty)
        {
            return OperationResult.Failed(ErrorDescriber.ObjectInvalidFailure());
        }

        try
        {
            return OperationResult.SuccessWithPayload(
                await DbContext.FindAsync<TEntity>(new object[] { id }, cancellationToken));
        }
        catch (Exception)
        {
            return OperationResult.Failed(ErrorDescriber.DatabaseSelectionFailure());
        }
    }
}