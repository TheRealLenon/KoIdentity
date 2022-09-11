using Microsoft.EntityFrameworkCore;
using Tekoding.KoIdentity.Abstraction.Extensions;
using Tekoding.KoIdentity.Abstraction.Models;

namespace Tekoding.KoIdentity.Abstraction;

/// <summary>
/// Provides the generic abstraction of the <see cref="Microsoft.EntityFrameworkCore"/> database context used by
/// KoIdentity.
/// </summary>
public class DatabaseContext<TDefaultEntity> : DbContext
    where TDefaultEntity : Entity
{
    private string DatabaseSchema { get; }

    /// <summary>
    /// Creates a new instance of the <see cref="DatabaseContext{TDefaultEntity}"/>-
    /// </summary>
    /// <param name="dbContextOptions">
    /// The <see cref="DbContextOptions"/> being used by a <see cref="DbContext"/>.
    /// </param>
    /// <param name="databaseSchema">The database schema name used by the database.</param>
    protected DatabaseContext(DbContextOptions dbContextOptions, string databaseSchema = "KoIdentity.Abstraction") :
        base(dbContextOptions)
    {
        DatabaseSchema = databaseSchema;
    }

    /// <summary>
    /// Further configuration of the model that was discovered by convention from the entity types exposed in
    /// <see cref="DbSet{TEntity}"/> properties on the derived context.
    /// </summary>
    /// <param name="modelBuilder">
    /// The builder being used to construct the model for this context. Databases (and other extensions) typically
    /// define the extension methods on this object that allows you to configure aspects of the model that are specific
    /// to a given database.
    /// </param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TDefaultEntity>().BuildDefaultSqlEntity();
        
        modelBuilder.HasDefaultSchema(DatabaseSchema);
        
        base.OnModelCreating(modelBuilder);
    }

    /// <inheritdoc />
    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var entityEntries = ChangeTracker.Entries().Where(entry =>
            entry.Entity is Entity && entry.State is EntityState.Added or EntityState.Modified);

        foreach (var entityEntry in entityEntries)
        {
            if (entityEntry.State == EntityState.Added)
            {
                (entityEntry.Entity as Entity)!.CreationDate = DateTime.UtcNow;
                (entityEntry.Entity as Entity)!.ChangeDate = DateTime.MinValue;
            }
            else
            {
                (entityEntry.Entity as Entity)!.ChangeDate = DateTime.UtcNow;
            }
        }
        return base.SaveChangesAsync(cancellationToken);
    }

#nullable disable
    /// <summary>
    /// Gets or sets the <see cref="DbSet{TEntity}"/> of the main entity being used by this
    /// <see cref="DatabaseContext{TDefaultEntity}"/>.
    /// </summary>
    protected DbSet<TDefaultEntity> MainEntities { get; set; }
#nullable restore
}