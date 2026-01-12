using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using ProjectCore.Application.UseCases.Permissions.Scan;

namespace ProjectCore.Presentation.MVC.Permissions
{
    public sealed class MvcPermissionScanner : IPermissionScanner
    {
        public IEnumerable<PermissionScanResult> Scan()
        {
            var controllerTypes = Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(t =>
                    typeof(Controller).IsAssignableFrom(t) &&
                    !t.IsAbstract);

            foreach (var controller in controllerTypes)
            {
                var module = controller.Name.Replace("Controller", "");

                var actions = controller
                    .GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly)
                    .Where(m =>
                        !m.IsDefined(typeof(NonActionAttribute)) &&
                        IsValidActionMethod(m));

                foreach (var action in actions)
                {
                    yield return new PermissionScanResult
                    {
                        Module = module,
                        Action = action.Name
                    };
                }
            }
        }

        private static bool IsValidActionMethod(MethodInfo method)
        {
            // Loại bỏ method đặc biệt: get_, set_, operator...
            if (method.IsSpecialName)
                return false;

            // Chỉ chấp nhận IActionResult hoặc Task<IActionResult>
            var returnType = method.ReturnType;

            if (typeof(IActionResult).IsAssignableFrom(returnType))
                return true;

            if (returnType.IsGenericType &&
                returnType.GetGenericTypeDefinition() == typeof(Task<>) &&
                typeof(IActionResult).IsAssignableFrom(returnType.GetGenericArguments()[0]))
                return true;

            return false;
        }


    }


}
