using Microsoft.AspNetCore.Authentication.Cookies;
using ProjectCore.Application.UseCases.Users.Commands.Login;
using System.Security.Claims;


namespace ProjectCore.Presentation.MVC.Authentication.Claims
{
    public static class ClaimsPrincipalBuilder
    {
        public static ClaimsPrincipal Build(LoginUserResult user)
        {
            var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.Email, user.Email)
        };

            foreach (var permission in user.Permissions)
            {
                claims.Add(new Claim("permission", permission));
            }

            var identity = new ClaimsIdentity(
                claims,
                CookieAuthenticationDefaults.AuthenticationScheme);

            return new ClaimsPrincipal(identity);
        }
    }

}
