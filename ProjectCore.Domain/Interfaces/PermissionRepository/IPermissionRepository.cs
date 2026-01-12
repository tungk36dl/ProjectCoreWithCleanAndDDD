using ProjectCore.Domain.Entities;

namespace ProjectCore.Domain.Interfaces.PermissionRepository
{
    public interface IPermissionRepository
    {
        Task<bool> ExistsAsync(string module, string action, CancellationToken ct);
        Task AddAsync(Permission permission, CancellationToken ct);
        Task<IEnumerable<Permission>> GetAllAsync(CancellationToken ct = default);
    }
}
