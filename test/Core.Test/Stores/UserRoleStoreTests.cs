using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using Tekoding.KoIdentity.Abstraction.Errors;
using Tekoding.KoIdentity.Abstraction.Stores;
using Tekoding.KoIdentity.Core.Models;
using Tekoding.KoIdentity.Core.Stores;
using Tekoding.KoIdentity.Core.Test.Helpers;
using Tekoding.KoIdentity.Core.Validations;

namespace Tekoding.KoIdentity.Core.Test.Stores;

/// <summary>
/// Defines a set of integration tests for the <see cref="UserRoleStore"/>.
/// </summary>
public class UserRoleStoreTests
{
#nullable disable

    private readonly UserRoleValidator _userRoleValidator = new();
    private readonly UserValidator _userValidator = new();
    private readonly RoleValidator _roleValidator = new();

    private readonly DbContextOptions _dbContextOptions = new DbContextOptionsBuilder().UseSqlServer(
        Environment.GetEnvironmentVariable("TekodingAzureDEVConnection") ??
        throw new InvalidOperationException()).Options;

#nullable restore

    #region SetUp & TearDown

    /// <summary>
    /// Provides a common set of functions that are performed just before each test method is called.
    /// </summary>
    /// <remarks>
    /// If a setup method fails or throws an exception, the test is not executed and a failure or error is reported.
    /// </remarks>
    /// <exception cref="InvalidOperationException">
    /// The exception will be thrown, when the connection string was not found.
    /// </exception>
    [SetUp]
    public async Task Setup()
    {
        await DatabaseMocker.LoadDatabase(_dbContextOptions);
    }

    /// <summary>
    /// Provides a common set of functions that are performed immediately after each test method is run.
    /// </summary>
    /// <remarks>The method is guaranteed to be called, even if an exception is thrown.</remarks>
    [TearDown]
    public async Task Teardown()
    {
        await DatabaseMocker.ResetDatabase(_dbContextOptions);
    }

    #endregion


    /// <summary>
    /// Checking the <see cref="UserStore"/> to inherit correctly from <see cref="EntityStore{TEntity}"/> and
    /// <seealso cref="IEntityStore{TEntity}"/>.
    /// </summary>
    [Test]
    public void UserRoleStore_IsAssignableToEntityStore()
    {
        var ctx = new DatabaseContext(_dbContextOptions);
        var userStore = new UserRoleStore(ctx, _userRoleValidator);

        userStore.Should().BeAssignableTo<EntityStore<UserRole<User>>>().And
            .BeAssignableTo<IEntityStore<UserRole<User>>>();
    }

    #region GetRolesByUserId

    /// <summary>
    /// Checking the <see cref="UserRoleStore"/> to indicate a failed operation, if the roles could not be selected due
    /// to an <b>ObjectInvalidFailure</b>.
    /// </summary>
    [Test]
    public async Task GetRolesByUserId_FailsWithObjectInvalidFailure()
    {
        await using var ctx = new DatabaseContext(_dbContextOptions);
        IUserRoleStore userRoleStore = new UserRoleStore(ctx, _userRoleValidator);

        var getRolesByUserIdResult = await userRoleStore.GetRolesByUserId(Guid.Empty);

        getRolesByUserIdResult.State.Should().BeFalse();
        getRolesByUserIdResult.ErrorCount.Should().Be(1);
        getRolesByUserIdResult.Payload.Should().NotBeNull().And.BeOfType<Error[]>();
        getRolesByUserIdResult.ToString().Should()
            .ContainAll(nameof(ErrorDescriber.ObjectInvalidFailure));
    }

    /// <summary>
    /// Checking the <see cref="UserRoleStore"/> to indicate a failed operation, if the roles could not be selected
    /// due to an <b>DatabaseSelectionFailure</b>.
    /// </summary>
    [Test]
    public async Task GetRolesByUserId_FailsWithDatabaseSelectionFailure()
    {
        await using var ctx = new DatabaseContext(_dbContextOptions);
        IUserRoleStore userRoleStore = new UserRoleStore(ctx, _userRoleValidator);

        await ctx.DisposeAsync();

        var selectionResult = await userRoleStore.GetRolesByUserId(Guid.NewGuid());

        selectionResult.State.Should().BeFalse();
        selectionResult.ErrorCount.Should().Be(1);
        selectionResult.Payload.Should().NotBeNull().And.BeOfType<Error[]>();
        selectionResult.ToString().Should()
            .ContainAll(nameof(ErrorDescriber.DatabaseSelectionFailure));
    }

    /// <summary>
    /// Checking the <see cref="UserRoleStore"/> to indicate a succeeded operation, if the roles could be retrieved from
    /// the database.
    /// </summary>
    [Test]
    public async Task GetRolesByUserId_Succeeds()
    {
        await using var ctx = new DatabaseContext(_dbContextOptions);
        IUserStore userStore = new UserStore(ctx, _userValidator);
        IRoleStore roleStore = new RoleStore(ctx, _roleValidator);
        IUserRoleStore userRoleStore = new UserRoleStore(ctx, _userRoleValidator);

        var user = new User
        {
            Username = "Username",
            Password = "Password"
        };
        await userStore.CreateAsync(user);

        var role = new Role
        {
            Name = "RoleName"
        };
        await roleStore.CreateAsync(role);

        var userRole = new UserRole<User>
        {
            UserId = user.Id,
            RoleId = role.Id,
        };
        await userRoleStore.CreateAsync(userRole);

        var selectionResult = await userRoleStore.GetRolesByUserId(user.Id);

        selectionResult.State.Should().BeTrue();
        selectionResult.ErrorCount.Should().Be(0);
        selectionResult.Payload.Should().NotBeNull().And.BeOfType<List<Role>>();
        selectionResult.Payload.As<List<Role>>().First().Id.Should().Be(userRole.RoleId);
        selectionResult.ToString().Should().Contain("Succeeded");
    }

    #endregion

    #region GetUsersByRoleId

    /// <summary>
    /// Checking the <see cref="UserRoleStore"/> to indicate a failed operation, if the users could not be selected due
    /// to an <b>ObjectInvalidFailure</b>.
    /// </summary>
    [Test]
    public async Task GetUsersByRoleId_FailsWithObjectInvalidFailure()
    {
        await using var ctx = new DatabaseContext(_dbContextOptions);
        IUserRoleStore userRoleStore = new UserRoleStore(ctx, _userRoleValidator);

        var getUsersByRoleIdResult = await userRoleStore.GetUsersByRoleId(Guid.Empty);

        getUsersByRoleIdResult.State.Should().BeFalse();
        getUsersByRoleIdResult.ErrorCount.Should().Be(1);
        getUsersByRoleIdResult.Payload.Should().NotBeNull().And.BeOfType<Error[]>();
        getUsersByRoleIdResult.ToString().Should()
            .ContainAll(nameof(ErrorDescriber.ObjectInvalidFailure));
    }

    /// <summary>
    /// Checking the <see cref="UserRoleStore"/> to indicate a failed operation, if the users could not be selected
    /// due to an <b>DatabaseSelectionFailure</b>.
    /// </summary>
    [Test]
    public async Task GetUsersByRoleId_FailsWithDatabaseSelectionFailure()
    {
        await using var ctx = new DatabaseContext(_dbContextOptions);
        IUserRoleStore userRoleStore = new UserRoleStore(ctx, _userRoleValidator);

        await ctx.DisposeAsync();

        var selectionResult = await userRoleStore.GetUsersByRoleId(Guid.NewGuid());

        selectionResult.State.Should().BeFalse();
        selectionResult.ErrorCount.Should().Be(1);
        selectionResult.Payload.Should().NotBeNull().And.BeOfType<Error[]>();
        selectionResult.ToString().Should()
            .ContainAll(nameof(ErrorDescriber.DatabaseSelectionFailure));
    }

    /// <summary>
    /// Checking the <see cref="UserRoleStore"/> to indicate a succeeded operation, if the users could be retrieved from
    /// the database.
    /// </summary>
    [Test]
    public async Task GetUsersByRoleId_Succeeds()
    {
        await using var ctx = new DatabaseContext(_dbContextOptions);
        IUserStore userStore = new UserStore(ctx, _userValidator);
        IRoleStore roleStore = new RoleStore(ctx, _roleValidator);
        IUserRoleStore userRoleStore = new UserRoleStore(ctx, _userRoleValidator);

        var user = new User
        {
            Username = "Username",
            Password = "Password"
        };
        await userStore.CreateAsync(user);

        var role = new Role
        {
            Name = "RoleName"
        };
        await roleStore.CreateAsync(role);

        var userRole = new UserRole<User>
        {
            UserId = user.Id,
            RoleId = role.Id,
        };
        await userRoleStore.CreateAsync(userRole);

        var selectionResult = await userRoleStore.GetUsersByRoleId(role.Id);

        selectionResult.State.Should().BeTrue();
        selectionResult.ErrorCount.Should().Be(0);
        selectionResult.Payload.Should().NotBeNull().And.BeOfType<List<User>>();
        selectionResult.Payload.As<List<User>>().First().Id.Should().Be(userRole.UserId);
        selectionResult.ToString().Should().Contain("Succeeded");
    }

    #endregion
}