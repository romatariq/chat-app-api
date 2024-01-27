using App.Contracts.DAL.IRepositories;
using App.Domain.Enums;
using App.DTO.Private.Shared;
using App.Helpers;
using Base.DAL.EF;
using Base.Helpers;
using Microsoft.EntityFrameworkCore;
using Dal = App.DTO.Private.DAL;
using Domain = App.Domain;

namespace App.DAL.EF.Repositories;

public class CommentRepository: EfBaseRepository<Domain.Comment, AppDbContext>, ICommentRepository
{
    public CommentRepository(AppDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<(IEnumerable<Dal.Comment> comments, int totalPageCount)> GetAll(GetAllCommentsParameters parameters)
    {
        var commentsQuery = DbSet
            .Include(c => c.Url)
            .ThenInclude(u => u!.WebDomain)
            .Include(c => c.User)
            .Include(c => c.CommentReactions)
            .Where(c =>
                c.GroupId == parameters.GroupId &&
                c.Url!.WebDomain!.Name == parameters.Domain &&
                c.Url.Path == parameters.Path &&
                c.Url.Params == parameters.Parameters)
            .SortComments(parameters.Sort)
            .Select(c => new Dal.Comment
            {
                Id = c.Id,
                CreatedAtUtc = c.CreatedAtUtc,
                Text = c.Text,
                Username = c.User!.UserName!,
                Likes = c.CommentReactions!
                    .Count(cr =>
                        cr.ReactionType == ECommentReactionType.Like),
                Dislikes = c.CommentReactions!
                    .Count(cr =>
                        cr.ReactionType == ECommentReactionType.Dislike),
                HasUserLiked = c.CommentReactions!
                    .Any(cr =>
                        cr.ReactionType == ECommentReactionType.Like &&
                        cr.UserId == parameters.UserId),
                HasUserDisliked = c.CommentReactions!
                    .Any(cr =>
                        cr.ReactionType == ECommentReactionType.Dislike &&
                        cr.UserId == parameters.UserId)
            })
            .AsQueryable();

        var comments = await commentsQuery
            .Paging(parameters.PageNr, parameters.PageSize)
            .ToListAsync();
        var totalCommentsCount = await commentsQuery.CountAsync();
        return (comments, totalCommentsCount.GetPageCount(parameters.PageSize));
    }

    public async Task<Dal.Comment> Add(Guid urlId, Guid groupId, Guid userId, string text, string username)
    {
        var comment = new Domain.Comment
        {
            Text = text,
            GroupId = groupId,
            UserId = userId,
            UrlId = urlId
        };
        await DbSet.AddAsync(comment);

        return new Dal.Comment
        {
            Id = comment.Id,
            CreatedAtUtc = comment.CreatedAtUtc,
            Text = comment.Text,
            Username = username
        };
    }
}