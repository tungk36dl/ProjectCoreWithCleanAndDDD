using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectCore.Application.UseCases.Users.Commands.Login
{
    public class LoginUserCommand
    {
        public string UserNameOrEmail { get; set; }
        public string Password { get; set; }
        public string RememberMe { get; set; }
    }
}
