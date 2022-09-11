using System;
using System.Collections.Generic;
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
    private readonly BaseEntityValidator _baseEntityValidator = new();
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
        IEntityStore<BaseEntity> entityStore = new BaseEntityStore(ctx, _baseEntityValidator);

        var entity = new BaseEntity();

        var entityCountBeforeCreation = await ctx.Set<BaseEntity>().CountAsync();
        var creationResult = await entityStore.CreateAsync(entity);
        var entityCountAfterCreation = await ctx.Set<BaseEntity>().CountAsync();

        creationResult.State.Should().BeTrue();
        creationResult.Payload.Should().BeNull();
        creationResult.ErrorCount.Should().Be(0);
        creationResult.ToString().Should().Contain("Succeeded");
        entityCountAfterCreation.Should().Be(entityCountBeforeCreation + 1);
    }

    /// <summary>
    /// Checking the <see cref="IEntityStore{TEntity}"/> to indicate a failed operation, if the entity was not created
    /// due to an <b>DatabaseCreationFailure</b>.
    /// </summary>
    [Test]
    public async Task CreateAsync_FailsWithDatabaseCreationFailure()
    {
        await using var ctx = new DatabaseContext(_dbContextOptions);
        IEntityStore<BaseEntity> entityStore = new BaseEntityStore(ctx, _baseEntityValidator);

        var entity = new BaseEntity();
        await entityStore.CreateAsync(entity);

        var entityCountBeforeCreation = await ctx.Set<BaseEntity>().CountAsync();
        var creationResult = await entityStore.CreateAsync(entity);
        var entityCountAfterCreation = await ctx.Set<BaseEntity>().CountAsync();

        creationResult.State.Should().BeFalse();
        creationResult.ErrorCount.Should().Be(1);
        creationResult.Payload.Should().NotBeNull().And.BeOfType<Error[]>();
        creationResult.ToString().Should()
            .ContainAll(nameof(ErrorDescriber.DatabaseCreationFailure));
        entityCountAfterCreation.Should().Be(entityCountBeforeCreation);
    }

    /// <summary>
    /// Checking the <see cref="IEntityStore{TEntity}"/> to indicate a successful operation, if the entity was
    /// successfully updated in the database.
    /// </summary>
    [Test]
    public async Task UpdateAsync_Succeeds()
    {
        await using var ctx = new DatabaseContext(_dbContextOptions);
        IEntityStore<BaseEntity> entityStore = new BaseEntityStore(ctx, _baseEntityValidator);

        var entity = new BaseEntity();
        await entityStore.CreateAsync(entity);
        await Task.Delay(100);

        entity.TestProp = "New Updated Value";

        var updateResult = await entityStore.UpdateAsync(entity);

        updateResult.State.Should().BeTrue();
        updateResult.Payload.Should().BeNull();
        updateResult.ErrorCount.Should().Be(0);
        updateResult.ToString().Should().Contain("Succeeded");
        entity.ChangeDate.Should().BeAfter(entity.CreationDate);
    }

    /// <summary>
    /// Checking the <see cref="IEntityStore{TEntity}"/> to indicate a failed operation, if the entity was not updated
    /// due to an <b>DatabaseUpdateFailure</b>.
    /// </summary>
    [Test]
    public async Task UpdateAsync_FailsWithDatabaseUpdateFailure()
    {
        await using var ctx = new DatabaseContext(_dbContextOptions);
        IEntityStore<BaseEntity> entityStore = new BaseEntityStore(ctx, _baseEntityValidator);

        var firstEntityToCreate = new BaseEntity { TestProp = "Unique" };
        var secondEntityToCreate = new BaseEntity();
        await entityStore.CreateAsync(firstEntityToCreate);
        await entityStore.CreateAsync(secondEntityToCreate);

        secondEntityToCreate.TestProp = "Unique";

        var updateResult = await entityStore.UpdateAsync(secondEntityToCreate);

        updateResult.State.Should().BeFalse();
        updateResult.ErrorCount.Should().Be(1);
        updateResult.Payload.Should().NotBeNull().And.BeOfType<Error[]>();
        updateResult.ToString().Should()
            .ContainAll(nameof(ErrorDescriber.DatabaseUpdateFailure));
    }

    /// <summary>
    /// Checking the <see cref="IEntityStore{TEntity}"/> to indicate a failed operation, if the entity was not updated
    /// due to an <b>ObjectStateUnmodifiedFailure</b>.
    /// </summary>
    [Test]
    public async Task UpdateAsync_FailsWithObjectStateUnmodifiedFailure()
    {
        await using var ctx = new DatabaseContext(_dbContextOptions);
        IEntityStore<BaseEntity> entityStore = new BaseEntityStore(ctx, _baseEntityValidator);

        var entity = new BaseEntity();

        var updateResult = await entityStore.UpdateAsync(entity);

        updateResult.State.Should().BeFalse();
        updateResult.ErrorCount.Should().Be(1);
        updateResult.Payload.Should().NotBeNull().And.BeOfType<Error[]>();
        updateResult.ToString().Should()
            .ContainAll(nameof(ErrorDescriber.ObjectStateUnmodifiedFailure));
    }

    /// <summary>
    /// Checking the <see cref="IEntityStore{TEntity}"/> to indicate a successful operation, if the entity was
    /// successfully deleted from the database.
    /// </summary>
    [Test]
    public async Task DeleteAsync_Succeeds()
    {
        await using var ctx = new DatabaseContext(_dbContextOptions);
        IEntityStore<BaseEntity> entityStore = new BaseEntityStore(ctx, _baseEntityValidator);

        var entity = new BaseEntity();
        await entityStore.CreateAsync(entity);

        var entityCountBeforeDeletion = await ctx.Set<BaseEntity>().CountAsync();
        var deletionResult = await entityStore.DeleteAsync(entity);
        var entityCountAfterDeletion = await ctx.Set<BaseEntity>().CountAsync();

        deletionResult.State.Should().BeTrue();
        deletionResult.Payload.Should().BeNull();
        deletionResult.ErrorCount.Should().Be(0);
        deletionResult.ToString().Should().Contain("Succeeded");
        entityCountAfterDeletion.Should().Be(entityCountBeforeDeletion - 1);
    }

    /// <summary>
    /// Checking the <see cref="IEntityStore{TEntity}"/> to indicate a failed operation, if the entity was not updated
    /// due to an <b>DatabaseDeletionFailure</b>.
    /// </summary>
    [Test]
    public async Task DeleteAsync_FailsWithDatabaseDeletionFailure()
    {
        await using var ctx = new DatabaseContext(_dbContextOptions);
        IEntityStore<BaseEntity> entityStore = new BaseEntityStore(ctx, _baseEntityValidator);

        var entity = new BaseEntity();

        var deletionResult = await entityStore.DeleteAsync(entity);

        deletionResult.State.Should().BeFalse();
        deletionResult.ErrorCount.Should().Be(1);
        deletionResult.Payload.Should().NotBeNull().And.BeOfType<Error[]>();
        deletionResult.ToString().Should()
            .ContainAll(nameof(ErrorDescriber.DatabaseDeletionFailure));
    }

    /// <summary>
    /// Checking the <see cref="IEntityStore{TEntity}"/> to indicate a failed operation, if the entity was not updated
    /// due to an <b>DatabaseSelectionFailure</b>.
    /// </summary>
    [Test]
    public async Task GetAllAsync_FailsWithDatabaseSelectionFailure()
    {
        await using var ctx = new DatabaseContext(_dbContextOptions);
        IEntityStore<BaseEntity> entityStore = new BaseEntityStore(ctx, _baseEntityValidator);

        await ctx.DisposeAsync();

        var selectionResult = await entityStore.GetAllAsync();

        selectionResult.State.Should().BeFalse();
        selectionResult.ErrorCount.Should().Be(1);
        selectionResult.Payload.Should().NotBeNull().And.BeOfType<Error[]>();
        selectionResult.ToString().Should()
            .ContainAll(nameof(ErrorDescriber.DatabaseSelectionFailure));
    }

    /// <summary>
    /// Checking the <see cref="IEntityStore{TEntity}"/> to indicate a successful operation, if all entities were
    /// successfully loaded from the database.
    /// </summary>
    [Test]
    public async Task GetAllAsync_Succeeds()
    {
        await using var ctx = new DatabaseContext(_dbContextOptions);
        IEntityStore<BaseEntity> entityStore = new BaseEntityStore(ctx, _baseEntityValidator);

        var entity = new BaseEntity();
        await entityStore.CreateAsync(entity);

        var selectionResult = await entityStore.GetAllAsync();

        selectionResult.State.Should().BeTrue();
        selectionResult.ErrorCount.Should().Be(0);
        selectionResult.Payload.Should().NotBeNull().And.BeOfType<List<BaseEntity>>();
        selectionResult.Payload.As<List<BaseEntity>>().Count.Should().Be(await ctx.Set<BaseEntity>().CountAsync());
        selectionResult.ToString().Should().Contain("Succeeded");
    }

    /// <summary>
    /// Checking the <see cref="IEntityStore{TEntity}"/> to indicate a failed operation, if the entity was not updated
    /// due to an <b>DatabaseSelectionFailure</b>.
    /// </summary>
    [Test]
    public async Task FindByIdAsync_FailsWithDatabaseSelectionFailure()
    {
        await using var ctx = new DatabaseContext(_dbContextOptions);
        IEntityStore<BaseEntity> entityStore = new BaseEntityStore(ctx, _baseEntityValidator);

        await ctx.DisposeAsync();

        var findByIdResult = await entityStore.FindByIdAsync(Guid.NewGuid());

        findByIdResult.State.Should().BeFalse();
        findByIdResult.ErrorCount.Should().Be(1);
        findByIdResult.Payload.Should().NotBeNull().And.BeOfType<Error[]>();
        findByIdResult.ToString().Should()
            .ContainAll(nameof(ErrorDescriber.DatabaseSelectionFailure));
    }

    /// <summary>
    /// Checking the <see cref="IEntityStore{TEntity}"/> to indicate a successful operation, if a single entity was
    /// successfully loaded from the database.
    /// </summary>
    [Test]
    public async Task FindByIdAsync_Succeeds()
    {
        await using var ctx = new DatabaseContext(_dbContextOptions);
        IEntityStore<BaseEntity> entityStore = new BaseEntityStore(ctx, _baseEntityValidator);

        var entity = new BaseEntity();
        await entityStore.CreateAsync(entity);

        var selectionResult = await entityStore.FindByIdAsync(entity.Id);

        selectionResult.State.Should().BeTrue();
        selectionResult.ErrorCount.Should().Be(0);
        selectionResult.Payload.Should().NotBeNull().And.BeOfType<BaseEntity>();
        selectionResult.Payload.As<BaseEntity>().Should().Be(entity);
        selectionResult.ToString().Should().Contain("Succeeded");
    }

    /// <summary>
    /// Checking the <see cref="IEntityStore{TEntity}"/> to indicate a failed operation, if the entity was not updated
    /// due to an <b>ObjectInvalidFailure</b>.
    /// </summary>
    [Test]
    public async Task FindByIdAsync_FailsWithObjectInvalidFailure()
    {
        await using var ctx = new DatabaseContext(_dbContextOptions);
        IEntityStore<BaseEntity> entityStore = new BaseEntityStore(ctx, _baseEntityValidator);

        var findByIdResult = await entityStore.FindByIdAsync(Guid.Empty);

        findByIdResult.State.Should().BeFalse();
        findByIdResult.ErrorCount.Should().Be(1);
        findByIdResult.Payload.Should().NotBeNull().And.BeOfType<Error[]>();
        findByIdResult.ToString().Should()
            .ContainAll(nameof(ErrorDescriber.ObjectInvalidFailure));
    }
}