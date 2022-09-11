// KoIdentity Copyright (C) 2022 Tekoding. All Rights Reserved.
// 
// Created: 2022.05.08
// 
// Authors: TheRealLenon
// 
// Licensed under the MIT License. See LICENSE.md in the project root for license
// information.
// 
// KoIdentity is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the MIT
// License for more details.

using Microsoft.EntityFrameworkCore;
using Tekoding.KoIdentity.Abstraction.Errors;
using Tekoding.KoIdentity.Abstraction.Extensions;
using Tekoding.KoIdentity.Abstraction.Models;
using Tekoding.KoIdentity.Abstraction.Validations;

namespace Tekoding.KoIdentity.Abstraction.Stores;

/// <summary>
/// Represents the abstracted store implementation for <typeparamref name="TEntity"/>s.
/// </summary>
/// <typeparam name="TEntity">The type encapsulating an <see cref="Entity"/>.</typeparam>
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
    /// <param name="entityValidator">The validator used for validating the <typeparamref name="TEntity"/>s.</param>
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
        catch (Exception)
        {
            return OperationResult.Failed(ErrorDescriber.DatabaseCreationFailure());
        }

        return OperationResult.Success;
    }

    /// <inheritdoc />
    public async Task<OperationResult> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var entityValidationResult = await EntityValidator.ValidateAsync(entity, cancellationToken);

        if (!entityValidationResult.IsValid)
        {
            return OperationResult.Failed(entityValidationResult.TransformValidationFailuresToErrors());
        }

        if (DbContext.Entry(entity).State != EntityState.Modified)
        {
            return OperationResult.Failed(ErrorDescriber.ObjectStateUnmodifiedFailure());
        }

        DbContext.Attach(entity);
        DbContext.Update(entity);

        try
        {
            await DbContext.SaveChangesAsync(cancellationToken);
        }
        catch
        {
            return OperationResult.Failed(ErrorDescriber.DatabaseUpdateFailure());
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
    public async Task<OperationResult> GetAllAsync(CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        try
        {
            return OperationResult.SuccessWithPayload(await DbContext.Set<TEntity>().ToListAsync(cancellationToken));
        }
        catch (Exception)
        {
            return OperationResult.Failed(ErrorDescriber.DatabaseSelectionFailure());
        }
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