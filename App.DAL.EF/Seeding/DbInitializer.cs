using System.Security.Claims;
using App.Domain;
using App.Domain.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic.FileIO;

namespace App.DAL.EF.Seeding;

public static class DbInitializer
{
    
    private static readonly Guid AdminId = Guid.Parse("bc7458ac-cbb0-4ecd-be79-d5abf19f8c77");
    

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
    
    public static void SeedIdentity(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
    {
        SeedRoles(roleManager);
        (Guid id, string email, string password) userData = (AdminId, "admin@app.com", "Foo.bar1");
        var user = userManager.FindByEmailAsync(userData.email).Result;
        if (user != null) return;
        
        user = new AppUser()
        {
            Id = userData.id,
            Email = userData.email,
            UserName = userData.email,
            EmailConfirmed = true,
            FirstName = "Admin",
            LastName = "App",
            IsVerified = true
        };
        var result = userManager.CreateAsync(user, userData.password).Result;
        if (!result.Succeeded)
        {
            throw new ApplicationException($"Cannot seed users, {result}");
        }
        
        var roleAddResult = userManager.AddToRoleAsync(user, "admin").Result;
        if (!roleAddResult.Succeeded)
        {
            throw new ApplicationException($"Cannot add role to admin, {result}");
        }
        
        userManager.AddClaimsAsync(user, new List<Claim>()
        {
            new(ClaimTypes.GivenName, user.FirstName),
            new(ClaimTypes.Surname, user.LastName)
        }).Wait();
    }
    
}