using System.ComponentModel.DataAnnotations;

namespace App.DTO.Public.v1;

public class Comment
{
    public Guid Id { get; set; }
    
    [MaxLength(1000)]
    public string Text { get; set; } = default!;

    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

    public string Username { get; set; } = default!;

    public int Likes { get; set; }
    
    public int Dislikes { get; set; }

    public bool HasUserLiked { get; set; }
 
    public bool HasUserDisliked { get; set; }

    public int RepliesCount { get; set; }

    public string? ReplyToUsername { get; set; }
    public Guid? ReplyToCommentId { get; set; }
    public Guid? ParentCommentId { get; set; }
}