using Base.Domain;

namespace App.DTO.Private.BLL;

public class Message: DomainEntityId
{
    public string Text { get; set; } = default!;

    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

    public string Username { get; set; } = default!;
}