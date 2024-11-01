using Microsoft.AspNetCore.Http;
using AppX.Client.Application.Users.Authorization.CustomClaimTypes;
using System.Security.Claims;

namespace AppX.Client.Application.Users
{
    public class UserContext(IHttpContextAccessor httpContextAccessor) : IUserContext
    {
        public CurrentUser? GetCurrentUser()
        {
            var user = httpContextAccessor?.HttpContext?.User;

            if (user == null)
            {
                throw new InvalidOperationException("User context is not present.");
            }

            if (user.Identity == null || !user.Identity.IsAuthenticated)
            {
                return null;
            }

            var userId = user.FindFirst(c => c.Type == ClaimTypes.NameIdentifier)!.Value;
            var email = user.FindFirst(c => c.Type == ClaimTypes.Email)!.Value;
            var roles = user.Claims.Where(c => c.Type == ClaimTypes.Role)!.Select(c => c.Value);
            var nationality = user.FindFirst(c => c.Type == UserClaimTypes.Nationality)!.Value;
            var dob = user.FindFirst(c => c.Type == UserClaimTypes.DateOfBirth)!.Value;
            var dobVal = dob == null ? (DateOnly?)null : DateOnly.ParseExact(dob, "yyyy-MM-dd");

            return new CurrentUser(userId, email, roles, nationality, dobVal);
        }

    }
}
