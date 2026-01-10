using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectCore.Application.UseCases.Roles.Commands.UpdateRole
{
    public sealed class UpdateRoleCommand
    {
        public Guid Id { get; init; }
        public string? Name { get; init; }
        public string? Description { get; init; }
        
        public Guid UpdatedBy { get; init; }
    }
}
