using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using Tekoding.KoIdentity.Abstraction.Stores;
using Tekoding.KoIdentity.Core.Models;
using Tekoding.KoIdentity.Core.Stores;
using Tekoding.KoIdentity.Core.Validations;

namespace Tekoding.KoIdentity.Core.Test.Stores;

/// <summary>
/// Defines a set of integration tests for the <see cref="UserStore"/>.
/// </summary>
public class UserStoreTests
{
#nullable disable
    
    private readonly UserValidator _userValidator = new();
    private readonly DbContextOptions _dbContextOptions = new DbContextOptionsBuilder().UseSqlServer(
        Environment.GetEnvironmentVariable("TekodingAzureDEVConnection") ??
    throw new InvalidOperationException()).Options;
    
#nullable restore
    
    /// <summary>
    /// Checking the <see cref="UserStore"/> to inherit correctly from <see cref="EntityStore{TEntity}"/> and
    /// <seealso cref="IEntityStore{TEntity}"/>.
    /// </summary>
    [Test]
    public void UserStore_IsAssignableToEntityStore()
    {
        var ctx = new DatabaseContext(_dbContextOptions);
        var userStore = new UserStore(ctx, _userValidator);

        userStore.Should().BeAssignableTo<EntityStore<User>>().And.BeAssignableTo<IEntityStore<User>>();
    }
}