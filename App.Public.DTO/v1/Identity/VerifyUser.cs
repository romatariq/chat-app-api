namespace App.Public.DTO.v1.Identity;

public class VerifyUser
{
    public string Email { get; set; } = default!;
    
    public bool IsVerified { get; set; }
}