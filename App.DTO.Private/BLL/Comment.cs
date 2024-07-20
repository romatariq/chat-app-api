using Base.Contracts.Domain;

namespace App.DTO.Private.BLL;

public class Comment : IDomainEntityId
{
    public Guid Id { get; set; }
    
    public string Text { get; set; } = default!;

    public DateTime CreatedAtUtc { get; set; }

    public string Username { get; set; } = default!;

    public int Likes { get; set; }
    
    public int Dislikes { get; set; }

    public bool HasUserLiked { get; set; }
 
    public bool HasUserDisliked { get; set; }

    public int RepliesCount { get; set; }

    public string? ReplyToUsername { get; set; }
}