using System.ComponentModel.DataAnnotations;

namespace App.Public.DTO.v1;

public class PostComment
{
    [MinLength(1)] [MaxLength(1000)] public string Text { get; set; } = default!;
}