using ProjectCore.Domain.Entities;
using ProjectCore.Models.Entities;

namespace ProjectCore.Models
{
    public class Role : DomainEntity<Guid>
    {
        public string Name { get; private set; }
        public string? Description { get; private set; }

        private readonly List<RolePermission> _permissions = new();
        public IReadOnlyCollection<RolePermission> RolePermissions => _permissions.AsReadOnly();

        protected Role() { }

        public Role(Guid id, string name, Guid createdBy)
            : base(id, createdBy)
        {
            Name = name;
        }

        public void AddPermission(Guid permissionId)
        {
            if (_permissions.Any(x => x.PermissionId == permissionId))
                return;

            _permissions.Add(new RolePermission(Id, permissionId));
        }

        public void RemovePermission(Guid permissionId)
        {
            var permission = _permissions.FirstOrDefault(x => x.PermissionId == permissionId);
            if (permission != null)
                _permissions.Remove(permission);
        }
    }

}

