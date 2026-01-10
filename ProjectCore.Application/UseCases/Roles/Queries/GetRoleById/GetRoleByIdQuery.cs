using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectCore.Application.UseCases.Roles.Queries.GetRoleById
{
    public sealed class GetRoleByIdQuery
    {
        public Guid Id { get; init; }
    }
}
