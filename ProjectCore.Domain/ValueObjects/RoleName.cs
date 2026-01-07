using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectCore.Domain.ValueObjects
{
    public sealed record RoleName
    {
        public string Value { get; }

        public RoleName(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Role name is required.");

            if (value.Length > 50)
                throw new ArgumentException("Role name is too long.");

            Value = value.Trim();
        }

        public override string ToString() => Value;
    }

}
