using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Tekoding.KoIdentity.Abstraction.Extensions;

namespace Tekoding.KoIdentity.Abstraction.Test.Helper;

internal class DatabaseContext : DatabaseContext<BaseEntity>
{
    internal DatabaseContext(DbContextOptions dbContextOptions, string databaseSchema = "KoIdentity.Abstraction.Tests") :
        base(dbContextOptions, databaseSchema)
    {
    }

    /// <inheritdoc />
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BaseEntity>().BuildUniqueSqlIndex(e => e.TestProp);
        
        base.OnModelCreating(modelBuilder);
    }
    
    private sealed class DatabaseContextFactory : IDesignTimeDbContextFactory<DatabaseContext>
    {
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