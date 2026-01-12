using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectCore.Application.UseCases.Users.Commands.CreateUser
{
    public sealed class CreateUserCommand
    {
        // init
        public string UserName { get; init; }
        public string Email { get; init; }
        public string PasswordHash { get; init; }
        public Guid? CreatedBy { get; set; }
        public List<Guid> RoleIds { get; init; } = new();
    }

}
