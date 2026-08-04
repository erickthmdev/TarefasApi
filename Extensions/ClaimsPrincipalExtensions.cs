using System.Security.Claims;

namespace TarefasApi.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static int GetUserId(this ClaimsPrincipal user)
            => int.Parse(user.FindFirstValue(ClaimTypes.NameIdentifier)!);
    }
}
