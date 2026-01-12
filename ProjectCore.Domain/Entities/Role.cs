using ProjectCore.Domain.Entities;
using ProjectCore.Domain.ValueObjects.Role;

namespace ProjectCore.Domain.Entities
{

    public class Role : DomainEntity<Guid>
    {
        public RoleName Name { get; private set; }
        public string? Description { get; private set; }

        private readonly List<RolePermission> _permissions = new();
        public IReadOnlyCollection<RolePermission> RolePermissions => _permissions;

        protected Role() { }

        public Role(Guid id, RoleName name, Guid createdBy)
            : base(id, createdBy)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
        }
        public Role(Guid id, RoleName name, string? description, Guid createdBy)
       : base(id, createdBy)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Description = description;
        }

        public void UpdateDetails(string? name, string? description,  Guid updatedBy)
        {
            if (!string.IsNullOrWhiteSpace(name))
            {
                Name = new RoleName(name);
            }
            Description = description;
            SetUpdatedBy(updatedBy);
            SetUpdatedDate(DateTime.UtcNow);
        }
        public void SetUpdatedDate(DateTime updatedDate)
        {
            UpdatedDate = updatedDate;
        }

        public void SetUpdatedBy(Guid updatedBy)
        {
            UpdatedBy = updatedBy;
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

