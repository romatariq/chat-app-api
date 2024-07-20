using System.ComponentModel.DataAnnotations;

namespace App.DTO.Public.v1;

public class PostComment
{
    [MinLength(1)] [MaxLength(1000)] public string Text { get; set; } = default!;

    public Guid GroupId { get; set; }

    public string Url { get; set; } = default!;
}