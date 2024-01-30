using App.Domain.Enums;
using Base.Domain;

namespace App.DTO.Private.BLL;

public class CommentReaction: DomainEntityId
{
    public ECommentReactionType ReactionType { get; set; }

    public Guid CommentId { get; set; }

    public Guid UserId { get; set; }
}