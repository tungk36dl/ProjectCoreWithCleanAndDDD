using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using ProjectCore.Application.Dtos.Users;
using ProjectCore.Domain.Entities;
using ProjectCore.Presentation.MVC.Authentication.Claims;

namespace ProjectCore.Presentation.MVC.Authentication.SignIn
{
    public sealed class CookieSignInService : ICookieSignInService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserClaimsFactory _claimsFactory;

        public CookieSignInService(
            IHttpContextAccessor httpContextAccessor,
            IUserClaimsFactory claimsFactory)
        {
            _httpContextAccessor = httpContextAccessor;
            _claimsFactory = claimsFactory;
        }

        public async Task SignInAsync(AuthUserDto user)
        {
            var principal = _claimsFactory.Create(user);

            await _httpContextAccessor.HttpContext!
                .SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    principal,
                    new AuthenticationProperties
                    {
                        IsPersistent = true,
                        ExpiresUtc = DateTimeOffset.UtcNow.AddHours(8)
                    });
        }

        public async Task SignOutAsync()
        {
            await _httpContextAccessor.HttpContext!
                .SignOutAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme);
        }
    }

}
