using ProjectCore.Application.Dtos.Permissions;
using ProjectCore.Application.Mappings;
using ProjectCore.Domain.Interfaces.PermissionRepository;

namespace ProjectCore.Application.UseCases.Permissions.Queries.GetAllPermissions
{
    public class GetAllPermissionsHandler
    {
        private readonly IPermissionRepository _permissionRepository;

        public GetAllPermissionsHandler(IPermissionRepository permissionRepository)
        {
            _permissionRepository = permissionRepository;
        }

        public async Task<IEnumerable<PermissionDto>> Handle(CancellationToken cancellationToken = default)
        {
            var permissions = await _permissionRepository.GetAllAsync(cancellationToken);
            return permissions.Select(permission => PermissionMapper.ToDto(permission));
        }
    }
}
