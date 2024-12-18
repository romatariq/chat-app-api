using App.Domain.Enums;
using Base.Domain;

namespace App.DTO.Private.DAL;

public class CommentReaction: DomainEntityId
{
    public ECommentReactionType ReactionType { get; set; }

    public Guid CommentId { get; set; }

    public Guid UserId { get; set; }
}