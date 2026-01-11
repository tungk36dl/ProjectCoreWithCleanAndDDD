using ProjectCore.Application.Common.Models;
using ProjectCore.Application.Dtos.Roles;
using ProjectCore.Application.Mappings;
using ProjectCore.Domain.Interfaces.RoleRepository;

namespace ProjectCore.Application.UseCases.Roles.Queries.GetDataRoles
{
    public class GetDataRolesHandler
    {
        private readonly IRoleRepository _roleRepository;

        public GetDataRolesHandler(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        public async Task<PagedResult<RoleDto>> Handle(
            RoleSearch search, 
            CancellationToken cancellationToken)
        {
            var (roles, count) = await _roleRepository.GetDataAsync(search, cancellationToken);
            var roleDtos = roles.Select(RoleMapper.ToDto).ToList();
            return new PagedResult<RoleDto>(roleDtos, count, search.Page, search.PageSize);
        }
    }
}
