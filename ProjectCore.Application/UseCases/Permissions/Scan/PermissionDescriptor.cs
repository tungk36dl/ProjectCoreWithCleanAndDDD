using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectCore.Application.UseCases.Permissions.Scan
{
    public sealed record PermissionDescriptor(
      string Module,
      string Action
  );
}
