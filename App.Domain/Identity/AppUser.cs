using Base.Contracts.Domain;
using Microsoft.AspNetCore.Identity;

namespace App.Domain.Identity;

public class AppUser: IdentityUser<Guid>, IDomainEntityId
{
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
    
    public bool IsVerified { get; set; }
}