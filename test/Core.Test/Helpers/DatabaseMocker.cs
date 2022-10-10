using Microsoft.EntityFrameworkCore;
using Tekoding.KoIdentity.Core.Models;

namespace Tekoding.KoIdentity.Core.Test.Helpers;

internal static class DatabaseMocker
{
    private static List<User> _databaseDumpUsers = new();
    private static List<Role> _databaseDumpRoles = new();
    private static List<UserRole<User>> _databaseDumpUserRoles = new();

    internal static async Task LoadDatabase(DbContextOptions dbContextOptions)
    {
        await using var ctx = new DatabaseContext(dbContextOptions);
        _databaseDumpUsers = new List<User>(await ctx.Set<User>().ToListAsync());
        _databaseDumpRoles = new List<Role>(await ctx.Set<Role>().ToListAsync());
        _databaseDumpUserRoles = new List<UserRole<User>>(await ctx.Set<UserRole<User>>().ToListAsync());
    }

    internal static async Task ResetDatabase(DbContextOptions dbContextOptions)
    {
        await ResetUsers(dbContextOptions);
        await ResetRoles(dbContextOptions);
        await ResetUserRoles(dbContextOptions);
    }

    private static async Task ResetUsers(DbContextOptions dbContextOptions)
    {
        await using var ctx = new DatabaseContext(dbContextOptions);
        ctx.Set<User>().RemoveRange(await ctx.Set<User>().ToListAsync());
        await ctx.AddRangeAsync(_databaseDumpUsers);
        await ctx.SaveChangesAsync();
    }
    
    private static async Task ResetRoles(DbContextOptions dbContextOptions)
    {
        await using var ctx = new DatabaseContext(dbContextOptions);
        ctx.Set<Role>().RemoveRange(await ctx.Set<Role>().ToListAsync());
        await ctx.AddRangeAsync(_databaseDumpRoles);
        await ctx.SaveChangesAsync();
    }
    
    private static async Task ResetUserRoles(DbContextOptions dbContextOptions)
    {
        await using var ctx = new DatabaseContext(dbContextOptions);
        ctx.Set<UserRole<User>>().RemoveRange(await ctx.Set<UserRole<User>>().ToListAsync());
        await ctx.AddRangeAsync(_databaseDumpUserRoles);
        await ctx.SaveChangesAsync();
    }
}