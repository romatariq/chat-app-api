using System.ComponentModel.DataAnnotations;
using App.Domain.Enums;

namespace App.DTO.Public.v1;

public class CommentReaction
{
    [Required]
    public Guid CommentId { get; set; }

    public ECommentReactionType ReactionType { get; set; }
}