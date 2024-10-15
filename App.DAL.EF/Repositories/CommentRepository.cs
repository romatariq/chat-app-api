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
            .Include(c => c.ParentCommentReplies)
            .Where(c =>
                c.ParentCommentId == null &&
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
                HasUserLiked = parameters.UserId != null && c.CommentReactions!
                    .Any(cr =>
                        cr.UserId == parameters.UserId &&
                        cr.ReactionType == ECommentReactionType.Like),
                HasUserDisliked = parameters.UserId != null && c.CommentReactions!
                    .Any(cr =>
                        cr.UserId == parameters.UserId &&
                        cr.ReactionType == ECommentReactionType.Dislike),
                RepliesCount = c.ParentCommentReplies!.Count,
                ReplyToCommentId = c.ReplyToCommentId,
                ParentCommentId = c.ParentCommentId
            })
            .AsQueryable();

        var comments = await commentsQuery
            .Paging(parameters.PageNr, parameters.PageSize)
            .ToListAsync();
        var totalCommentsCount = await commentsQuery.CountAsync();
        return (comments, totalCommentsCount.GetPageCount(parameters.PageSize));
    }

    public async Task<(IEnumerable<Dal.Comment> comments, int totalPageCount)> GetAllReplies(Guid parentCommentId, Guid? userId, ESort sort, int pageSize, int pageNr)
    {
        var commentsQuery = DbSet
            .Include(c => c.User)
            .Include(c => c.CommentReactions)
            .Include(c => c.ReplyToComment!.User)
            .Where(c => c.ParentCommentId == parentCommentId)
            .SortComments(sort)
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
                HasUserLiked = userId != null && c.CommentReactions!
                    .Any(cr =>
                        cr.UserId == userId &&
                        cr.ReactionType == ECommentReactionType.Like),
                HasUserDisliked = userId != null && c.CommentReactions!
                    .Any(cr =>
                        cr.UserId == userId &&
                        cr.ReactionType == ECommentReactionType.Dislike),
                ReplyToUsername = c.ReplyToComment!.User!.UserName,
                ReplyToCommentId = c.ReplyToCommentId,
                ParentCommentId = c.ParentCommentId
            })
            .AsQueryable();

        var comments = await commentsQuery
            .Paging(pageNr, pageSize)
            .ToListAsync();
        var totalCommentsCount = await commentsQuery.CountAsync();
        return (comments, totalCommentsCount.GetPageCount(pageSize));
    }

    public async Task<Dal.Comment> Add(Guid urlId, Guid userId, string text)
    {
        var comment = new Domain.Comment
        {
            Text = text,
            UserId = userId,
            UrlId = urlId,
        };
        return await Add(comment);
    }

    public async Task<Dal.Comment> AddReply(Guid parentCommentId, Guid replyToCommentId, Guid userId, string text)
    {
        var reply = await DbSet
            .Include(c => c.User)
            .Where(c => c.Id == replyToCommentId)
            .FirstAsync();

        var comment = new Domain.Comment
        {
            Text = text,
            UserId = userId,
            UrlId = reply.UrlId,
            ParentCommentId = parentCommentId,
            ReplyToCommentId = replyToCommentId
        };

        var addedComment = await Add(comment);
        addedComment.ReplyToUsername = reply.User!.UserName;
        
        return addedComment;
    }

    private async Task<Dal.Comment> Add(Domain.Comment comment)
    {
        await DbSet.AddAsync(comment);
        return new Dal.Comment
        {
            Id = comment.Id,
            CreatedAtUtc = comment.CreatedAtUtc,
            Text = comment.Text,
            ReplyToCommentId = comment.ReplyToCommentId,
            ParentCommentId = comment.ParentCommentId
        };
    }
}