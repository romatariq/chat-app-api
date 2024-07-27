using App.Contracts.DAL.IRepositories;
using App.Domain.Enums;
using App.DTO.Common;
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
            .Include(c => c.CommentReplies)
            .Where(c =>
                c.ParentCommentId == null &&
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
                        cr.UserId == parameters.UserId),
                RepliesCount = c.CommentReplies!.Count
            })
            .AsQueryable();

        var comments = await commentsQuery
            .Paging(parameters.PageNr, parameters.PageSize)
            .ToListAsync();
        var totalCommentsCount = await commentsQuery.CountAsync();
        return (comments, totalCommentsCount.GetPageCount(parameters.PageSize));
    }

    public async Task<(IEnumerable<Dal.Comment> comments, int totalPageCount)> GetAllReplies(Guid parentCommentId, Guid userId, int pageSize, int pageNr)
    {
        var commentsQuery = DbSet
            .Include(c => c.User)
            .Include(c => c.CommentReactions)
            .Include(c => c.ReplyToComment!.User)
            .Where(c => c.ParentCommentId == parentCommentId)
            .SortComments(ESort.Old)
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
                        cr.UserId == userId),
                HasUserDisliked = c.CommentReactions!
                    .Any(cr =>
                        cr.ReactionType == ECommentReactionType.Dislike &&
                        cr.UserId == userId),
                ReplyToUsername = c.ReplyToComment!.User!.UserName
            })
            .AsQueryable();

        var comments = await commentsQuery
            .Paging(pageNr, pageSize)
            .ToListAsync();
        var totalCommentsCount = await commentsQuery.CountAsync();
        return (comments, totalCommentsCount.GetPageCount(pageSize));
    }

    public async Task<Dal.Comment> Add(Guid urlId, Guid groupId, Guid userId, string text)
    {
        return await Add(urlId, groupId, null, null, userId, text);
    }

    public async Task<Dal.Comment> AddReply(Guid parentCommentId, Guid replyToCommentId, Guid userId, string text)
    {
        return await Add(null, null, parentCommentId, replyToCommentId, userId, text);
    }

    private async Task<Dal.Comment> Add(Guid? urlId, Guid? groupId, Guid? parentCommentId, Guid? replyToCommentId, Guid userId, string text)
    {
        var comment = new Domain.Comment
        {
            Text = text,
            GroupId = groupId,
            UserId = userId,
            UrlId = urlId,
            ParentCommentId = parentCommentId,
            ReplyToCommentId = replyToCommentId
        };
        await DbSet.AddAsync(comment);
        var replyToUsername = replyToCommentId == null ? null : await DbSet
            .Include(c => c.User)
            .Where(c => c.Id == comment.ReplyToCommentId)
            .Select(c => c.User!.UserName)
            .FirstOrDefaultAsync();

        return new Dal.Comment
        {
            Id = comment.Id,
            CreatedAtUtc = comment.CreatedAtUtc,
            Text = comment.Text,
            ReplyToUsername = replyToUsername
        };
    }
}