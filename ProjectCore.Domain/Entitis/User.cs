using ProjectCore.Domain.Entities;
using ProjectCore.Domain.Entitis;


namespace ProjectCore.Models
{

    public class User : DomainEntity<Guid>
    {
        public string UserName { get; private set; }
        public string Email { get; private set; }
        public string PasswordHash { get; private set; }

        private readonly List<UserRole> _userRoles = new();
        public IReadOnlyCollection<UserRole> UserRoles => _userRoles.AsReadOnly();

        protected User() { }

        public User(Guid id, string userName, string email, string passwordHash, Guid createdBy)
            : base(id, createdBy)
        {
            UserName = userName;
            Email = email;
            PasswordHash = passwordHash;
        }

        public void AssignRole(Guid roleId, Guid assignedBy)
        {
            if (_userRoles.Any(x => x.RoleId == roleId))
                return;

            _userRoles.Add(new UserRole(Id, roleId, assignedBy));
        }

        public void RemoveRole(Guid roleId)
        {
            var role = _userRoles.FirstOrDefault(x => x.RoleId == roleId);
            if (role != null)
                _userRoles.Remove(role);
        }
    }

}
