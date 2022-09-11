using Microsoft.EntityFrameworkCore;
using Tekoding.KoIdentity.Abstraction.Stores;
using Tekoding.KoIdentity.Abstraction.Validations;

namespace Tekoding.KoIdentity.Abstraction.Test.Helper;

internal class BaseEntityStore : EntityStore<BaseEntity>
{
    internal BaseEntityStore(DbContext dbContext, EntityValidator<BaseEntity> entityValidator) : base(dbContext,
        entityValidator)
    {
    }
}