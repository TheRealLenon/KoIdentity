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

using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Tekoding.KoIdentity.Abstraction.Test.Helper;

internal class DatabaseContext : DatabaseContext<BaseEntity>
{
    internal DatabaseContext(DbContextOptions dbContextOptions, string databaseSchema = "KoIdentity.Abstraction.Tests") :
        base(dbContextOptions, databaseSchema)
    {
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