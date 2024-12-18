using System.ComponentModel.DataAnnotations;

namespace App.DTO.Public.v1
{
    public class PostReply
    {
        [MinLength(1)]
        [MaxLength(1000)]
        public string Text { get; set; } = default!;

        [Required]
        public Guid ParentCommentId { get; set; }

        [Required]
        public Guid ReplyToCommentId { get; set; }
    }
}
