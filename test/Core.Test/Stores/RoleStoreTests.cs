using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using Tekoding.KoIdentity.Abstraction.Stores;
using Tekoding.KoIdentity.Core.Models;
using Tekoding.KoIdentity.Core.Stores;
using Tekoding.KoIdentity.Core.Validations;

namespace Tekoding.KoIdentity.Core.Test.Stores;

/// <summary>
/// Defines a set of integration tests for the <see cref="RoleStore"/>.
/// </summary>
public class RoleStoreTests
{
#nullable disable
    
    private readonly RoleValidator _roleValidator = new();
    private readonly DbContextOptions _dbContextOptions = new DbContextOptionsBuilder().UseSqlServer(
        Environment.GetEnvironmentVariable("TekodingAzureDEVConnection") ??
        throw new InvalidOperationException()).Options;
    
#nullable restore
    
    /// <summary>
    /// Checking the <see cref="RoleStore"/> to inherit correctly from <see cref="EntityStore{TEntity}"/> and
    /// <seealso cref="IEntityStore{TEntity}"/>.
    /// </summary>
    [Test]
    public void RoleStore_IsAssignableToEntityStore()
    {
        var ctx = new DatabaseContext(_dbContextOptions);
        var roleStore = new RoleStore(ctx, _roleValidator);

        roleStore.Should().BeAssignableTo<EntityStore<Role>>().And.BeAssignableTo<IEntityStore<Role>>();
    }
}