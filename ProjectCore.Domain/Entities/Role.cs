using ProjectCore.Domain.Entities;
using ProjectCore.Domain.ValueObjects;

namespace ProjectCore.Domain.Entities
{
    public class Role : DomainEntity<Guid>
    {
        public RoleName Name { get; private set; }
        public string? Description { get; private set; }

        private readonly List<RolePermission> _permissions = new();
        public IReadOnlyCollection<RolePermission> RolePermissions => _permissions.AsReadOnly();

        protected Role() { }

        public Role(Guid id, RoleName name, Guid createdBy)
            : base(id, createdBy)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
        }

        public void AddPermission(Guid permissionId, Guid createdBy)
        {
            if (_permissions.Any(x => x.PermissionId == permissionId))
                return;

            _permissions.Add(new RolePermission(Id, permissionId, createdBy));
        }

        public void RemovePermission(Guid permissionId)
        {
            var permission = _permissions.FirstOrDefault(x => x.PermissionId == permissionId);
            if (permission != null)
                _permissions.Remove(permission);
        }
    }

}

