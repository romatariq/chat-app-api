using Base.Contracts.Domain;
using Microsoft.AspNetCore.Identity;

namespace App.Domain.Identity;

public class AppRole: IdentityRole<Guid>, IDomainEntity
{
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
}