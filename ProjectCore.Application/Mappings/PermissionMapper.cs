using ProjectCore.Application.Dtos.Permissions;
using ProjectCore.Domain.Entities;

namespace ProjectCore.Application.Mappings
{
    public static class PermissionMapper
    {
        public static PermissionDto ToDto(Permission permission)
        {
            if (permission == null) return null;
            return new PermissionDto
            {
                Id = permission.Id,
                Code = permission.Code.ToString(),
                Module = permission.Module,
                Action = permission.Action,
                Description = permission.Description
            };
        }
    }
}
