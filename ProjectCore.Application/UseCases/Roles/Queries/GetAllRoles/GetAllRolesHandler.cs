using ProjectCore.Application.Dtos.Roles;
using ProjectCore.Application.Mappings;
using ProjectCore.Domain.Interfaces.RoleRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectCore.Application.UseCases.Roles.Queries.GetAllRoles
{
    public class GetAllRolesHandler
    {
        private readonly IRoleRepository _roleRepository;

        public GetAllRolesHandler(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        public async Task<IEnumerable<RoleDto>> Handle(CancellationToken cancellationToken = default)
        {
            var roles = await _roleRepository.GetAllAsync(cancellationToken);
            return roles.Select(role => RoleMapper.ToDto(role));
        }
    }
}
