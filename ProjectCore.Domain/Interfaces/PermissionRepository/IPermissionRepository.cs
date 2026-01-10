using ProjectCore.Domain.Entities;
using ProjectCore.Domain.ValueObjects.Permission;

namespace ProjectCore.Domain.Interfaces.PermissionRepository
{
    public interface IPermissionRepository
    {
        Task<bool> ExistsAsync(string module, string action, CancellationToken ct);
        Task AddAsync(Permission permission, CancellationToken ct);
    }

}
