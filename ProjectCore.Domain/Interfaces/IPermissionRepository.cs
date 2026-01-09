using ProjectCore.Models.Entities;

namespace ProjectCore.Domain.Interfaces
{
    public interface IPermissionRepository
    {
        Task<bool> ExistsAsync(string module, string action, CancellationToken ct);
        Task AddAsync(Permission permission, CancellationToken ct);
    }

}
