using ProjectCore.Application.Dtos.Roles;
using ProjectCore.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectCore.Application.Mappings
{
    public static class RoleMapper
    {
        public static RoleDto ToDto(Role role)
        {
            if (role == null) return null;
            return new RoleDto
            {
                Id = role.Id,
                Name = role.Name.ToString(),
                Description = role.Description,
                CreatedBy = role.CreatedBy,
                CreatedDate = role.CreatedDate,
                UpdatedBy = role.UpdatedBy,
                UpdatedDate = role.UpdatedDate,
                PermissionIds = role.RolePermissions.Select(rp => rp.PermissionId).ToList()
            };
        }
    }
}
