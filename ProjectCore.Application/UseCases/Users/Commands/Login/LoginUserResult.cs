using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectCore.Application.UseCases.Users.Commands.Login
{
    public class LoginUserResult
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public IReadOnlyCollection<string> Roles { get; set; }
    }
}
