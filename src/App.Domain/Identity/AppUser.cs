using Base.Contracts.Domain;
using Microsoft.AspNetCore.Identity;

namespace App.Domain.Identity;

public class AppUser: IdentityUser<Guid>, IDomainEntity
{
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
    
    public ICollection<Comment>? Comments { get; set; }
    
    public ICollection<Message>? Messages { get; set; }

    public ICollection<CommentReaction>? CommentReactions { get; set; }

    public ICollection<DomainReport>? DomainReports { get; set; }
}