using System;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using Tekoding.KoIdentity.Abstraction.Errors;
using Tekoding.KoIdentity.Abstraction.Stores;
using Tekoding.KoIdentity.Abstraction.Test.Helper;

namespace Tekoding.KoIdentity.Abstraction.Test.Stores;

/// <summary>
/// Defines a set of integration tests for the <see cref="IEntityStore{TEntity}"/>.
/// </summary>
public class EntityStoreTests
{
#nullable disable
    private DbContextOptions _dbContextOptions;
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
        _dbContextOptions = new DbContextOptionsBuilder().UseSqlServer(
            Environment.GetEnvironmentVariable("TekodingAzureDEVConnection") ??
            throw new InvalidOperationException()).Options;

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
    /// Checking the <see cref="IEntityStore{TEntity}"/> to indicate a successful operation without any errors, if the
    /// entity was created successful.
    /// </summary>
    [Test]
    public async Task CreateAsync_Succeeds()
    {
        await using var ctx = new DatabaseContext(_dbContextOptions);
        IEntityStore<BaseEntity> entityStore = new BaseEntityStore(ctx, new BaseEntityValidator());

        var entityToCreate = new BaseEntity();

        var entityCountBeforeCreation = await ctx.Set<BaseEntity>().CountAsync();
        var dateBeforeCreation = DateTime.UtcNow;
        var creationResult = await entityStore.CreateAsync(entityToCreate);
        var entityCountAfterCreation = await ctx.Set<BaseEntity>().CountAsync();

        creationResult.State.Should().BeTrue();
        creationResult.Payload.Should().BeNull();
        creationResult.ErrorCount.Should().Be(0);
        creationResult.ToString().Should().Contain("Succeeded");
        entityToCreate.Id.Should().NotBe(Guid.Empty);
        entityToCreate.ChangeDate.Should().Be(DateTime.MinValue);
        entityToCreate.CreationDate.Should().BeOnOrAfter(dateBeforeCreation);
        entityCountAfterCreation.Should().Be(entityCountBeforeCreation + 1);
    }

    /// <summary>
    /// Checking the <see cref="IEntityStore{TEntity}"/> to indicate an failed operation with the error
    /// <b>ObjectNullFailure</b>, if the entity was not created because the validation failed.
    /// </summary>
    [Test]
    public async Task CreateAsync_ValidateDatabaseObjectNullFailure()
    {
        await using var ctx = new DatabaseContext(_dbContextOptions);
        IEntityStore<BaseEntity> entityStore = new BaseEntityStore(ctx, new BaseEntityValidator());

        BaseEntity? entityToCreate = null;

        var entityCountBeforeCreation = await ctx.Set<BaseEntity>().CountAsync();
#pragma warning disable CS8604
        var creationResult = await entityStore.CreateAsync(entityToCreate);
#pragma warning restore CS8604
        var entityCountAfterCreation = await ctx.Set<BaseEntity>().CountAsync();

        creationResult.State.Should().BeFalse();
        creationResult.Payload.Should().NotBeNull().And.BeOfType<Error[]>();
        creationResult.ErrorCount.Should().Be(1);
        creationResult.ToString().Should().Contain("ObjectNullFailure");
        entityCountAfterCreation.Should().Be(entityCountBeforeCreation);
    }

    /// <summary>
    /// Checking the <see cref="IEntityStore{TEntity}"/> to indicate an failed operation with the error
    /// <b>DatabaseCreationFailure</b>, if the entity was not created because the could not be created in the database.
    /// </summary>
    [Test]
    public async Task CreateAsync_ValidateDatabaseCreationFailure()
    {
        await using var ctx = new DatabaseContext(_dbContextOptions);
        IEntityStore<BaseEntity> entityStore = new BaseEntityStore(ctx, new BaseEntityValidator());

        var entityToCreate = new BaseEntity();
        await entityStore.CreateAsync(entityToCreate);

        var entityCountBeforeCreation = await ctx.Set<BaseEntity>().CountAsync();
        var creationResult = await entityStore.CreateAsync(entityToCreate);
        var entityCountAfterCreation = await ctx.Set<BaseEntity>().CountAsync();

        creationResult.State.Should().BeFalse();
        creationResult.Payload.Should().NotBeNull().And.BeOfType<Error[]>();
        creationResult.ErrorCount.Should().Be(1);
        creationResult.ToString().Should().Contain("DatabaseCreationFailure");
        entityCountAfterCreation.Should().Be(entityCountBeforeCreation);
    }
}