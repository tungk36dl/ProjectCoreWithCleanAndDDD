using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectCore.Application.UseCases.Roles.Commands.CreateRole
{
    public sealed class CreateRoleCommand
    {
        public string RoleName { get; init; }
        public string? Description { get; init; }
        public Guid CreatedBy { get; set; }
    }
}
