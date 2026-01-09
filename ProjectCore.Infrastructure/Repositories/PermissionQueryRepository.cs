using Microsoft.EntityFrameworkCore;
using ProjectCore.Application.Interfaces;
using ProjectCore.Domain.ValueObjects.Permission;
using ProjectCore.Infrastructure.Persistence;

namespace ProjectCore.Infrastructure.Repositories
{
    public sealed class PermissionQueryRepository : IPermissionQueryRepository
    {
        private readonly ApplicationDbContext _context;

        public PermissionQueryRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IReadOnlyList<PermissionCode>> GetPermissionsByUserIdAsync(
            Guid userId,
            CancellationToken cancellationToken)
        {
            return await (
                from ur in _context.UserRoles
                join rp in _context.RolePermissions on ur.RoleId equals rp.RoleId
                join p in _context.Permissions on rp.PermissionId equals p.Id
                where ur.UserId == userId
                select p.Code
            )
            .Distinct()
            .ToListAsync(cancellationToken);
        }
    }

}
