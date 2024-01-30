using App.Domain.Enums;

namespace App.DTO.Public.v1;

public class CommentReaction
{
    public Guid? Id { get; set; }
    
    public Guid? UserId { get; set; }

    public Guid CommentId { get; set; }

    public ECommentReactionType ReactionType { get; set; }
}