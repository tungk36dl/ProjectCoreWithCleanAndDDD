using ProjectCore.Application.Interfaces;
using ProjectCore.Domain.Entities;
using ProjectCore.Domain.Interfaces.PermissionRepository;

namespace ProjectCore.Application.UseCases.Permissions.Scan
{


    public sealed class SyncPermissionsHandler
    {
        private readonly IPermissionScanner _scanner;
        private readonly IPermissionRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public SyncPermissionsHandler(
            IPermissionScanner scanner,
            IPermissionRepository repository,
            IUnitOfWork unitOfWork)
        {
            _scanner = scanner;
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(Guid adminId, CancellationToken ct)
        {
            var permissions = _scanner.Scan();

            foreach (var p in permissions)
            {
                if (await _repository.ExistsAsync(p.Module, p.Action, ct))
                    continue;

                var permission = new Permission(
                    Guid.NewGuid(),
                    p.Module,
                    p.Action,
                    adminId);
                await _repository.AddAsync(permission, ct);
            }
            await _unitOfWork.SaveChangesAsync(ct);
        }
    }

}
