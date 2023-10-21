using System.ComponentModel.DataAnnotations;
using App.Domain.Identity;
using Base.Domain;

namespace App.Domain;

public class Comment: DomainEntityId
{
    [MaxLength(1000)]
    public string Text { get; set; } = default!;

    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;


    public Guid UrlId { get; set; }
    public Url? Url { get; set; }

    public Guid UserId { get; set; }
    public AppUser? User { get; set; }

    public Guid GroupId { get; set; }
    public Group? Group { get; set; }

    public ICollection<CommentReaction>? CommentReactions { get; set; }
}