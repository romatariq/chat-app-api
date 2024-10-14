using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using App.Domain.Identity;
using Base.Domain;

namespace App.Domain;

public class Comment: DomainEntityId
{
    [MaxLength(1000)]
    public string Text { get; set; } = default!;

    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;


    public Guid UserId { get; set; }
    public AppUser? User { get; set; }


    public Guid UrlId { get; set; }
    public Url? Url { get; set; }

    // if reply - must have ParentCommentId and ReplyToCommentId
    public Guid? ParentCommentId { get; set; }
    public Comment? ParentComment { get; set; }

    public Guid? ReplyToCommentId { get; set; }
    public Comment? ReplyToComment { get; set; }

    public ICollection<CommentReaction>? CommentReactions { get; set; }

    [InverseProperty(nameof(ParentComment))]
    public ICollection<Comment>? CommentReplies { get; set; }

    [InverseProperty(nameof(ReplyToComment))]
    public ICollection<Comment>? ReplyReplies { get; set; }
}