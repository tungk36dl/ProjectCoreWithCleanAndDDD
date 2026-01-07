using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectCore.Domain.ValueObjects
{
    public sealed record PermissionCode
    {
        public string Module { get; }
        public string Action { get; }

        public PermissionCode(string module, string action)
        {
            if (string.IsNullOrWhiteSpace(module))
                throw new ArgumentException("Module is required.");

            if (string.IsNullOrWhiteSpace(action))
                throw new ArgumentException("Action is required.");

            Module = module.Trim();
            Action = action.Trim();
        }

        public override string ToString() => $"{Module}.{Action}";
    }

}
