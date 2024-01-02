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

    public static async Task SeedIdentity(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager,
        string adminPassword)
    {
        await SeedRoles(roleManager);
        await SeedUsers(userManager, adminPassword);
    }

    public static async Task SeedData(AppDbContext ctx)
    {
        await SeedGroups(ctx);
        await ctx.SaveChangesAsync();
    }

    public static async Task SeedDevData(AppDbContext ctx)
    {
        await SeedDevUrls(ctx);
        await ctx.SaveChangesAsync();
        
        await SeedDevComments(ctx);
        await ctx.SaveChangesAsync();
    }

    private static async Task SeedRoles(RoleManager<AppRole> roleManager)
    {
        if (await roleManager.Roles.AnyAsync()) return;

        var roles = new List<AppRole>
        {
            new() { Name = "admin" }
        };
        foreach (var role in roles)
        {
            await roleManager.CreateAsync(role);
        }
    }

    private static async Task SeedUsers(UserManager<AppUser> userManager, string adminPassword)
    {
        (string email, string userName) adminData = ("admin@app.com", "Admin");
        var admin = await userManager.FindByEmailAsync(adminData.email);
        if (admin != null) return;

        admin = new AppUser
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

    private static async Task SeedGroups(AppDbContext ctx)
    {
        if (await ctx.Groups.AnyAsync()) return;

        var group = new Group
        {
            Name = "Public all chat",
            GroupType = EGroupType.All
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

    private static async Task SeedDevUrls(AppDbContext ctx)
    {
        if (await ctx.WebDomains.AnyAsync()) return;
        var domain = new WebDomain
        {
            Name = "localhost:3000"
        };
        await ctx.WebDomains.AddAsync(domain);

        if (await ctx.Urls.AnyAsync()) return;
        var url = new Url
        {
            WebDomainId = domain.Id
        };
        await ctx.Urls.AddAsync(url);
    }
    
    
    private static async Task SeedDevComments(AppDbContext ctx)
    {
        if (await ctx.Comments.AnyAsync()) return;

        var allChatId = await ctx.Groups
            .Where(g => g.GroupType == EGroupType.All)
            .Select(g => g.Id)
            .SingleAsync();

        var adminId = await ctx.Users
            .Where(u => u.UserName!.ToLower() == "admin")
            .Select(u => u.Id)
            .SingleAsync();

        var urlId = await ctx.Urls
            .Include(u => u.WebDomain)
            .Where(u => u.WebDomain!.Name == "localhost:3000" && u.Path == null)
            .Select(u => u.Id)
            .SingleAsync();

        var random = new Random();
        var comments = new List<Comment>();
        for (int i = 0; i < 20; i++)
        {
            comments.Add(new Comment()
            {
                UserId = adminId,
                GroupId = allChatId,
                UrlId = urlId,
                CreatedAtUtc = DateTime.UtcNow,
                Text =
                    "this is a comment 2 very long comment agdsad sa das das dsa dsa saddadasdsa f sadasudhoipaufgousaygfoasadasudhoipaufgousaygfoasadasudhoipaufgousaygfoa dhoipaufgousaygfo asadasud hoipau fgous aygfodhoipa ufgousaygfoasada sudhoipaufgousaygfodhoipaufgousaygfoasadasudhoi paufgousaygfodhoipaufgousaygfoasadasudhoipaufgousaygfo",
                CommentReactions = random.Next(10) < 5 ? null : new List<CommentReaction>()
                {
                    new()
                    {
                        UserId = adminId,
                        ReactionType = random.Next(10) < 5 ? ECommentReactionType.Like : ECommentReactionType.Dislike,
                        CreatedAtUtc = DateTime.UtcNow
                    } 
                },
            });
        }

        await ctx.Comments.AddRangeAsync(comments);
    }
}