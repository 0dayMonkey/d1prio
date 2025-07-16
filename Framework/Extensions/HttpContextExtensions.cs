using System.Security.Claims;

namespace Framework.Extensions
{
    public static class HttpContextExtensions
    {
        public static string GetUserId(this HttpContext context)
        {
            var claim = context.User.FindFirst(ClaimTypes.NameIdentifier);
            return (claim != null && !string.IsNullOrWhiteSpace(claim.Value)) ? claim.Value : context.Request.Headers["API-Client-Id"];
        }
    }
}
