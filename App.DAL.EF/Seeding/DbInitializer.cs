using App.Domain;
using App.Domain.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF.Seeding;

public static class DbInitializer
{
    

    public static void MigrateDatabase(AppDbContext ctx)
    {
        ctx.Database.Migrate();
    }

    public static void DropDatabase(AppDbContext ctx)
    {
        ctx.Database.EnsureDeleted();
    }
    
    private static void SeedRoles(RoleManager<AppRole> roleManager)
    {
        if (roleManager.Roles.Any()) return;
        var roles = new List<AppRole>
        {
            new() { Name = "admin" }
        };
        foreach (var role in roles)
        {
            roleManager.CreateAsync(role).Wait();
        }
    }
    
    public static async Task SeedIdentity(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager, string adminPassword)
    {
        SeedRoles(roleManager);
        (string email, string userName) adminData = ("admin@app.com", "Admin");
        var admin = await userManager.FindByEmailAsync(adminData.email);
        if (admin != null) return;
        
        admin = new AppUser()
        {
            Email = adminData.email,
            UserName = adminData.userName,
            EmailConfirmed = true, 
            IsVerified = true
        };
        var result = await userManager.CreateAsync(admin, adminPassword);
        if (!result.Succeeded)
        {
            throw new ApplicationException($"Cannot seed users, {result}");
        }
        
        var roleAddResult = await userManager.AddToRoleAsync(admin, "admin");
        if (!roleAddResult.Succeeded)
        {
            throw new ApplicationException($"Cannot add role to admin, {result}");
        }
    }
    
    public static async Task SeedData(AppDbContext ctx)
    {
        await SeedGroups(ctx);
    }
    
    private static async Task SeedGroups(AppDbContext ctx)
    {
        if (ctx.Groups.Any()) return;

        var group = new Group
        {
            Name = "Public all chat",
            GroupType = EGroupType.All,
        };

        await ctx.Groups.AddAsync(group);

        var groupUsers = await ctx.Users
            .Select(u => new GroupUser
            {
                GroupId = group.Id,
                UserId = u.Id
            })
            .ToListAsync();

        await ctx.GroupUsers.AddRangeAsync(groupUsers);
    }
    
}