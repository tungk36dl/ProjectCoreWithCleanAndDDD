using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectCore.Application.UseCases.Roles.Commands.DeleteRole
{
    public sealed class DeleteRoleCommand
    {
        public Guid Id { get; init; }
    }
}
