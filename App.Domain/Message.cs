using System.ComponentModel.DataAnnotations;
using App.Domain.Identity;
using Base.Domain;

namespace App.Domain;

public class Message: DomainEntity
{
    [MaxLength(100)]
    public string Text { get; set; } = default!;


    public Guid UrlId { get; set; }
    public Url? Url { get; set; }

    public Guid UserId { get; set; }
    public AppUser? User { get; set; }
}