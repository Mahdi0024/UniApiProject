using System.Security.Claims;
using UniApiProject.Exeptions;

namespace UniApiProject.Helpers;

public static class Helpers
{
    public static Guid GetUserId(this ClaimsPrincipal user)
    {
        var userIdClaim = user.Claims.FirstOrDefault(c => c.Type == "id");
        if (userIdClaim == null)
        {
            throw new TokenException("Token validation failed");
        }
        var userId = Guid.Parse(userIdClaim.Value);
        return userId;
    }
}
