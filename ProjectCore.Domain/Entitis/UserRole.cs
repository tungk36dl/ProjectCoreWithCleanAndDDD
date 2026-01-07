using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectCore.Domain.Entitis;

public class UserRole
{
    public Guid UserId { get; private set; }
    public Guid RoleId { get; private set; }

    public DateTime AssignedAt { get; private set; }
    public Guid AssignedBy { get; private set; }

    protected UserRole() { }

    internal UserRole(Guid userId, Guid roleId, Guid assignedBy)
    {
        UserId = userId;
        RoleId = roleId;
        AssignedBy = assignedBy;
        AssignedAt = DateTime.UtcNow;
    }
}

