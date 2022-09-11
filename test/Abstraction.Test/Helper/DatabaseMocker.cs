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

using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Tekoding.KoIdentity.Abstraction.Test.Helper;

internal static class DatabaseMocker
{
    private static List<BaseEntity> _databaseDumpBaseEntities = new();

    internal static async Task LoadDatabase(DbContextOptions dbContextOptions)
    {
        await using var ctx = new DatabaseContext(dbContextOptions);
        _databaseDumpBaseEntities = new List<BaseEntity>(await ctx.Set<BaseEntity>().ToListAsync());
    }

    internal static async Task ResetDatabase(DbContextOptions dbContextOptions)
    {
        await ResetEntities(dbContextOptions);
    }

    private static async Task ResetEntities(DbContextOptions dbContextOptions)
    {
        await using var ctx = new DatabaseContext(dbContextOptions);
        ctx.Set<BaseEntity>().RemoveRange(await ctx.Set<BaseEntity>().ToListAsync());
        await ctx.AddRangeAsync(_databaseDumpBaseEntities);
        await ctx.SaveChangesAsync();
    }
}