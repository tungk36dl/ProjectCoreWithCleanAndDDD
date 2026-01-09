using Microsoft.AspNetCore.Authentication.Cookies;
using ProjectCore.Application.Dtos.Users;
using ProjectCore.Domain.Entities;
using System.Security.Claims;

namespace ProjectCore.Presentation.MVC.Authentication.Claims
{
    public sealed class UserClaimsFactory : IUserClaimsFactory
    {
        public ClaimsPrincipal Create(AuthUserDto user)
        {
            var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.UserId.ToString()),
            new(ClaimTypes.Name, user.UserName),
            new(ClaimTypes.Email, user.Email)
        };

            foreach (var permission in user.Permissions)
            {
                claims.Add(new Claim("permission", permission));
            }

            return new ClaimsPrincipal(
                new ClaimsIdentity(
                    claims,
                    CookieAuthenticationDefaults.AuthenticationScheme));
        }
    }


}
