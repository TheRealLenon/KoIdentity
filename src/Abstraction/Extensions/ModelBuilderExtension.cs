using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tekoding.KoIdentity.Abstraction.Models;

namespace Tekoding.KoIdentity.Abstraction.Extensions;

/// <summary>
/// Relational database specific extension methods for the <see cref="EntityTypeBuilder{TEntity}"/>.
/// </summary>
public static class ModelBuilderExtension
{
    /// <summary>
    /// Building the default structure for entities within the KoIdentity eco system by setting the unique identifier as
    /// key.
    /// </summary>
    /// <param name="entityTypeBuilder">
    /// The <see cref="EntityTypeBuilder{TEntity}"/> being used to build the entity.
    /// </param>
    /// <typeparam name="TEntity">The type encapsulating an <see cref="Entity"/>.</typeparam>
    /// <returns>Returns the <see cref="EntityTypeBuilder{TEntity}"/> used to build the default entity.</returns>
    public static EntityTypeBuilder<TEntity> BuildDefaultSqlEntity<TEntity>(
        this EntityTypeBuilder<TEntity> entityTypeBuilder)
        where TEntity : Entity
    {
        entityTypeBuilder.HasKey(e => e.Id);

        entityTypeBuilder.ToTable(typeof(TEntity).GetNameWithoutGenericArity());

        return entityTypeBuilder;
    }

    /// <summary>
    /// Building an unique index for an entity model.
    /// </summary>
    /// <param name="entityTypeBuilder">
    /// The <see cref="EntityTypeBuilder{TEntity}"/> being used to build the entity.
    /// </param>
    /// <typeparam name="TEntity">The type encapsulating an <see cref="Entity"/>.</typeparam>
    /// <param name="indexExpression">
    /// The <see cref="Expression{TDelegate}"/> used to define the parameter(s) where the unique index should be.
    /// </param>
    /// <returns>Returns the <see cref="EntityTypeBuilder{TEntity}"/> used to build the unique index.</returns>
    public static EntityTypeBuilder<TEntity> BuildUniqueSqlIndex<TEntity>(
        this EntityTypeBuilder<TEntity> entityTypeBuilder, Expression<Func<TEntity, object?>> indexExpression)
        where TEntity : Entity
    {
        entityTypeBuilder.HasIndex(indexExpression).IsUnique();

        return entityTypeBuilder;
    }
}