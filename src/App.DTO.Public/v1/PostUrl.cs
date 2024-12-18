using System.ComponentModel.DataAnnotations;

namespace App.DTO.Public.v1
{
    public class PostUrl
    {
        [MaxLength(10000)]
        public string Url { get; set; } = default!;
    }
}
