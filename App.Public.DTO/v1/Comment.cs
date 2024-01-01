using System.ComponentModel.DataAnnotations;

namespace App.Public.DTO.v1;

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
}