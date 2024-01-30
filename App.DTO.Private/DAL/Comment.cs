using Base.Domain;

namespace App.DTO.Private.DAL;

public class Comment : DomainEntityId
{
    public string Text { get; set; } = default!;

    public DateTime CreatedAtUtc { get; set; }

    public string Username { get; set; } = default!;

    public int Likes { get; set; }
    
    public int Dislikes { get; set; }

    public bool HasUserLiked { get; set; }
 
    public bool HasUserDisliked { get; set; }
}