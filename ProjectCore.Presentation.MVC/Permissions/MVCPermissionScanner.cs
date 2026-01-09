using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using ProjectCore.Application.UseCases.Permissions.Scan;

namespace ProjectCore.Infrastructure.Permissions
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
                    .GetMethods(BindingFlags.Instance | BindingFlags.Public)
                    .Where(m =>
                        !m.IsDefined(typeof(NonActionAttribute)));

                foreach (var action in actions)
                {
                    // yield là từ khóa trong C# được sử dụng để trả về một chuỗi các giá trị từ một phương thức,
                    // thuộc tính hoặc trình lặp mà không cần phải tạo một tập hợp tạm thời để lưu trữ tất cả các giá trị đó.
                    yield return new PermissionScanResult
                    {
                        Module = module,
                        Action = action.Name
                    };
                }
            }
        }
    }


}
