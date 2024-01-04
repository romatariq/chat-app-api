using System.Security.Claims;

namespace Base.Helpers;

public static class IdentityHelpers
{
    public static Guid GetUserId(this ClaimsPrincipal user)
    {
        return Guid.Parse(
            user.Claims.Single(c => c.Type == ClaimTypes.NameIdentifier).Value);
    }
    
    public static string GetUsername(this ClaimsPrincipal user)
    {
        return user.Claims
            .Single(c => c.Type == ClaimTypes.Name)
            .Value;
    }
}