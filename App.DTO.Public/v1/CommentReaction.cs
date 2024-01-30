using App.Domain.Enums;

namespace App.DTO.Public.v1;

public class CommentReaction
{
    public Guid CommentId { get; set; }

    public ECommentReactionType ReactionType { get; set; }
}