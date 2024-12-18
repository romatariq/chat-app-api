using App.Domain;
using App.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF.Seeding;

internal static class DevDataInitializer
{
    public static async Task SeedDevUrls(AppDbContext ctx)
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
    
    
    public static async Task SeedDevComments(AppDbContext ctx)
    {
        if (await ctx.Comments.AnyAsync()) return;

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
    
    
    public static async Task SeedDevMessages(AppDbContext ctx)
    {
        if (await ctx.Messages.AnyAsync()) return;

        var adminId = await ctx.Users
            .Where(u => u.UserName!.ToLower() == "admin")
            .Select(u => u.Id)
            .SingleAsync();

        var urlId = await ctx.Urls
            .Include(u => u.WebDomain)
            .Where(u => u.WebDomain!.Name == "localhost:3000" && u.Path == null)
            .Select(u => u.Id)
            .SingleAsync();

        var messages = new List<Message>();
        for (int i = 0; i < 20; i++)
        {
            messages.Add(new Message()
            {
                CreatedAtUtc = DateTime.UtcNow,
                Text = "this is a demo message to localhost:3000 for development purposes.",
                UrlId = urlId,
                UserId = adminId
            });
        }

        await ctx.Messages.AddRangeAsync(messages);
    }
}