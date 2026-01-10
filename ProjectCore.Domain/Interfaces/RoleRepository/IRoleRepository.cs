using ProjectCore.Domain.Entities;
using ProjectCore.Domain.ValueObjects.Role;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectCore.Domain.Interfaces.RoleRepository
{
    public interface IRoleRepository
    {
        Task<Role?> GetByIdAsync(Guid roleId, CancellationToken cancellationToken = default);
        Task<Role?> GetByNameAsync(
            RoleName roleName,
            CancellationToken cancellationToken = default);
        Task<bool> ExistsByNameAsync(
            RoleName roleName,
            CancellationToken cancellationToken = default);
        Task AddAsync(Role role, CancellationToken cancellationToken = default);
        void Update(Role role);
        void Remove(Role role);
        Task<IEnumerable<Role>> GetAllAsync(CancellationToken cancellationToken = default);
    }
}
