using ProjectCore.Application.Dtos.Users;
using ProjectCore.Domain.Entities;

namespace ProjectCore.Presentation.MVC.Authentication.SignIn
{
    public interface ICookieSignInService
    {
        Task SignInAsync(AuthUserDto user);
        Task SignOutAsync();
    }

}
