using System.ComponentModel.DataAnnotations;

namespace App.DTO.Public.v1;

public class Message
{
    public Guid Id { get; set; }
    
    [MaxLength(100)]
    public string Text { get; set; } = default!;

    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

    public string Username { get; set; } = default!;
}