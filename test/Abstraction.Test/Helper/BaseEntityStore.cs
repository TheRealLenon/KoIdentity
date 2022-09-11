// KoIdentity Copyright (C) 2022 Tekoding. All Rights Reserved.
// 
// Created: 2022.05.29
// 
// Authors: TheRealLenon
// 
// Licensed under the MIT License. See LICENSE.md in the project root for license
// information.
// 
// KoIdentity is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the MIT
// License for more details.

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