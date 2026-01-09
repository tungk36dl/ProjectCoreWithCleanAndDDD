using ProjectCore.Application.Dtos.Users;
using System.Security.Claims;

namespace ProjectCore.Presentation.MVC.Authentication.Claims
{
    public interface IUserClaimsFactory
    {
        ClaimsPrincipal Create(AuthUserDto user);
    }

}
