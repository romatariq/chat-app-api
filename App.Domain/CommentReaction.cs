using App.Domain.Identity;
using Base.Domain;

namespace App.Domain;

public class CommentReaction: DomainEntityId
{
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

    public ECommentReactionType ReactionType { get; set; }


    public Guid CommendId { get; set; }
    public Comment? Comment { get; set; }

    public Guid UserId { get; set; }
    public AppUser? User { get; set; }
}