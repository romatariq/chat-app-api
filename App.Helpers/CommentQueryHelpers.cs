using App.Domain;
using App.Public.DTO.v1;
using Comment = App.Domain.Comment;

namespace App.Helpers;

public static class CommentQueryHelpers
{
    public static IQueryable<Comment> SortComments(this IQueryable<Comment> query, ESort sortBy)
    {
        return sortBy switch
        {
            ESort.New => query.SortByDateDesc(),
            ESort.Old => query.SortByDateAsc(),
            _ => query.SortByTop()
        };
    }
    
    
    private static IQueryable<Comment> SortByTop(this IQueryable<Comment> query)
    {
        return query
            .OrderByDescending(c =>
                c.CommentReactions!.Count(cr => cr.ReactionType == ECommentReactionType.Like)
                - c.CommentReactions!.Count(cr => cr.ReactionType == ECommentReactionType.Dislike));
    }
    
    private static IQueryable<Comment> SortByDateAsc(this IQueryable<Comment> query)
    {
        return query
            .OrderBy(c => c.CreatedAtUtc);
    }
    
    private static IQueryable<Comment> SortByDateDesc(this IQueryable<Comment> query)
    {
        return query
            .OrderByDescending(c => c.CreatedAtUtc);
    }
}