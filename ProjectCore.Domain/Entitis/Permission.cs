using ProjectCore.Domain.Entities;
using ProjectCore.Domain.ValueObjects;

namespace ProjectCore.Models.Entities
{
    public class Permission : DomainEntity<Guid>
    {
        public string Code { get; private set; }   // Ví dụ: USER_CREATE
        public string Module { get; private set; } // User
        public string Action { get; private set; } // Create
        public string? Description { get; private set; }

        protected Permission() { }

        public Permission(Guid id, string module, string action, Guid createdBy)
            : base(id, createdBy)
        {
            Module = module;
            Action = action;
            Code = $"{module.ToUpper()}_{action.ToUpper()}";
        }
    }

}
