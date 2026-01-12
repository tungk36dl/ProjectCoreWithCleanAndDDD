using System;

namespace ProjectCore.Application.Dtos.Permissions
{
    public sealed class PermissionDto
    {
        public Guid Id { get; set; }
        public string Code { get; set; } = null!;
        public string Module { get; set; } = null!;
        public string Action { get; set; } = null!;
        public string? Description { get; set; }
    }
}
