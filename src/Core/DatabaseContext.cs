using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Tekoding.KoIdentity.Abstraction;
using Tekoding.KoIdentity.Abstraction.Extensions;
using Tekoding.KoIdentity.Core.Models;

namespace Tekoding.KoIdentity.Core;

/// <summary>
/// Provides the default implementation of the <see cref="Microsoft.EntityFrameworkCore"/> database context used by
/// KoIdentity.
/// </summary>
public class DatabaseContext : DatabaseContext<User>
{
    /// <summary>
    /// Creates a new instance of the <see cref="DatabaseContext"/>.
    /// </summary>
    /// <param name="dbContextOptions">
    /// The <see cref="DbContextOptions"/> being used by a <see cref="DbContext"/>.
    /// </param>
    /// <param name="databaseSchema">The database schema name. By Default the schema will be "KoIdentity".</param>
    public DatabaseContext(DbContextOptions dbContextOptions, string databaseSchema = "KoIdentity") : base(
        dbContextOptions, databaseSchema)
    {
    }

    /// <inheritdoc />
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().BuildUniqueSqlIndex(u => u.Username);

        modelBuilder.Entity<Role>().BuildDefaultSqlEntity().BuildUniqueSqlIndex(r => r.Name);
        
        base.OnModelCreating(modelBuilder);
    }

#nullable disable

    /// <summary>
    /// Gets or sets the <see cref="DbSet{TEntity}"/> of users.
    /// </summary>
    /// <remarks>
    /// The <see cref="User"/> model will be our main entity.
    /// </remarks>
    public DbSet<User> Users
    {
        get => MainEntities;
        set => MainEntities = value;
    }
    
    /// <summary>
    /// Gets or sets the <see cref="DbSet{TEntity}"/> of roles.
    /// </summary>
    public DbSet<Role> Roles { get; set; }

#nullable restore

    /// <inheritdoc/>
    internal sealed class DatabaseContextFactory : IDesignTimeDbContextFactory<DatabaseContext>
    {
        /// <summary>
        /// Creates the <see cref="DatabaseContext"/>.
        /// </summary>
        /// <param name="args">Optional arguments, which are not used.</param>
        /// <returns>The <see cref="DatabaseContext"/>.</returns>
        public DatabaseContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder();

            optionsBuilder.UseSqlServer(
                Environment.GetEnvironmentVariable("TekodingAzureDEVConnection") ??
                throw new InvalidOperationException());
            return new DatabaseContext(optionsBuilder.Options);
        }
    }
}