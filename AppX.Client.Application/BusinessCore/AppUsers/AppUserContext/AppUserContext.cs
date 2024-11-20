using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using AppX.Client.Application.BusinessCore.AppUsers;
using AppX.Client.Application.BusinessCore.AppUsers.Authorization.CustomClaimTypes;

namespace AppX.Client.Application.BusinessCore.AppUsers.AppUserContext
{
    public class AppUserContext(IHttpContextAccessor httpContextAccessor) : IAppUserContext
    {
        public CurrentAppUser? GetCurrentAppUser()
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
            var nationality = user.FindFirst(c => c.Type == AppUserClaimTypes.Nationality)!.Value;
            var dob = user.FindFirst(c => c.Type == AppUserClaimTypes.DateOfBirth)!.Value;
            var dobVal = dob == null ? (DateOnly?)null : DateOnly.ParseExact(dob, "yyyy-MM-dd");

            return new CurrentAppUser(userId, email, roles, nationality, dobVal);
        }

    }
}
