using App.Contracts.DAL.IRepositories;
using App.Domain.Enums;
using App.DTO.Public.v1;
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

    public async Task<(IEnumerable<Dal.Comment> comments, int totalPageCount)> GetAll(Guid groupId, Guid userId, string domain, string? path, string? parameters, ESort sort, int pageNr, int pageSize)
    {
        var commentsQuery = DbSet
            .Include(c => c.Url)
            .ThenInclude(u => u!.WebDomain)
            .Include(c => c.User)
            .Include(c => c.CommentReactions)
            .Where(c =>
                c.GroupId == groupId &&
                c.Url!.WebDomain!.Name == domain &&
                c.Url.Path == path &&
                c.Url.Params == parameters)
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
                HasUserLiked = c.CommentReactions!
                    .Any(cr =>
                        cr.ReactionType == ECommentReactionType.Like &&
                        cr.UserId == userId),
                HasUserDisliked = c.CommentReactions!
                    .Any(cr =>
                        cr.ReactionType == ECommentReactionType.Dislike &&
                        cr.UserId == userId)
            })
            .AsQueryable();

        var comments = await commentsQuery
            .Skip((pageNr - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
        var totalCommentsCount = await commentsQuery.CountAsync();
        return (comments, totalCommentsCount.GetPageCount(pageSize));
    }
}