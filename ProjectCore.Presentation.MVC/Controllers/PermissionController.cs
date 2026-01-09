using global::ProjectCore.Application.UseCases.Permissions.Scan;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ProjectCore.Presentation.MVC.Controllers
{

    [Authorize(Policy = "PERMISSION_MANAGE")]
    public sealed class PermissionController : Controller
    {
        private readonly SyncPermissionsHandler _syncPermissionsHandler;

        public PermissionController(SyncPermissionsHandler syncPermissionsHandler)
        {
            _syncPermissionsHandler = syncPermissionsHandler;
        }

        /// <summary>
        /// Trang quản lý permission
        /// </summary>
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Scan controller + action và lưu permission
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Scan(CancellationToken cancellationToken)
        {
            var systemUserId = GetCurrentUserId();

            await _syncPermissionsHandler.Handle(
                systemUserId,
                cancellationToken);

            TempData["Success"] = "Permissions scanned successfully.";

            return RedirectToAction(nameof(Index));
        }

        private Guid GetCurrentUserId()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            return Guid.TryParse(userId, out var id)
                ? id
                : Guid.Empty;

        }

    }
}
