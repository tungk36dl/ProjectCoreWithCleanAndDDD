using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectCore.Application.UseCases.Users.Commands.DeleteUser
{
    public sealed class DeleteUserCommand
    {
        public Guid Id { get; init; }
    }
}
