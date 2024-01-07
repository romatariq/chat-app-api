using System.ComponentModel.DataAnnotations;
using Base.Contracts.Domain;

namespace App.Private.DTO.DAL;

public class Comment : IDomainEntityId
{
    public Guid Id { get; set; }
    
    [MaxLength(1000)]
    public string Text { get; set; } = default!;

    public DateTime CreatedAtUtc { get; set; }

    public string Username { get; set; } = default!;

    public int Likes { get; set; }
    
    public int Dislikes { get; set; }

    public bool HasUserLiked { get; set; }
 
    public bool HasUserDisliked { get; set; }
}