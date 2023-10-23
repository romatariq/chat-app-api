using System.ComponentModel.DataAnnotations;

namespace App.Public.DTO.v1.Identity;

public class Register
{
    [StringLength(128, MinimumLength = 1, ErrorMessage = "Incorrect length")]
    public string Email { get; set; } = default!;
    
    [StringLength(128, MinimumLength = 1, ErrorMessage = "Incorrect length")]
    public string Password { get; set; } = default!;    
    
    [RegularExpression("^[a-zA-Z0-9_-]+$", ErrorMessage = "Contains illegal symbols")]
    [StringLength(10, MinimumLength = 1, ErrorMessage = "Incorrect length")]
    public string UserName { get; set; } = default!;
}