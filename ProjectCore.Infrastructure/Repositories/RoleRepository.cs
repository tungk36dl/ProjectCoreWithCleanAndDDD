using Microsoft.EntityFrameworkCore;
using ProjectCore.Domain.Entities;
using ProjectCore.Domain.Interfaces.RoleRepository;
using ProjectCore.Domain.ValueObjects.Role;
using ProjectCore.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectCore.Infrastructure.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly ApplicationDbContext _context;

        public RoleRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Role?> GetByIdAsync(Guid roleId, CancellationToken cancellationToken = default)
        {
            return await _context.Roles.FirstOrDefaultAsync(x => x.Id == roleId, cancellationToken);
        }

        public async Task<Role?> GetByNameAsync(RoleName roleName, CancellationToken cancellationToken = default)
        {
            return await _context.Roles.FirstOrDefaultAsync(x => x.Name.Value == roleName.Value, cancellationToken);
        }

        public async Task<bool> ExistsByNameAsync(RoleName roleName, CancellationToken cancellationToken = default)
        {
            return await _context.Roles.AnyAsync(x => x.Name.Value == roleName.Value, cancellationToken);
        }

        public async Task AddAsync(Role role, CancellationToken cancellationToken = default)
        {
            await _context.Roles.AddAsync(role, cancellationToken);
        }

        public void Update(Role role)
        {
            _context.Roles.Update(role);
        }

        public void Remove(Role role)
        {
            _context.Roles.Remove(role);
        }

        public async Task<IEnumerable<Role>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Roles.ToListAsync(cancellationToken);
        }
    }
}
