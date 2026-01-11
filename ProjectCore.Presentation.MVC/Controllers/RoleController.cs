using Microsoft.AspNetCore.Mvc;
using ProjectCore.Application.UseCases.Roles.Commands.CreateRole;
using ProjectCore.Application.UseCases.Roles.Commands.DeleteRole;
using ProjectCore.Application.UseCases.Roles.Commands.UpdateRole;
using ProjectCore.Application.UseCases.Roles.Queries.GetAllRoles;
using ProjectCore.Application.UseCases.Roles.Queries.GetDataRoles;
using ProjectCore.Application.UseCases.Roles.Queries.GetRoleById;
using ProjectCore.Domain.Interfaces.RoleRepository;
using ProjectCore.Presentation.MVC.Models.Roles;

namespace ProjectCore.Presentation.MVC.Controllers
{
    public class RoleController : BaseController
    {
        private readonly ILogger<RoleController> _logger;
        private readonly GetAllRolesHandler _getAllRolesHandler;
        private readonly GetDataRolesHandler _getDataRolesHandler;
        private readonly CreateRoleHandler _createRoleHandler;
        private readonly UpdateRoleHandler _updateRoleHandler;
        private readonly DeleteRoleHandler _deleteRoleHandler;
        private readonly GetRoleByIdHandler _getRoleByIdHandler;

        public RoleController(
            ILogger<RoleController> logger,
            GetAllRolesHandler getAllRolesHandler,
            GetDataRolesHandler getDataRolesHandler,
            CreateRoleHandler createRoleHandler,
            UpdateRoleHandler updateRoleHandler,
            DeleteRoleHandler deleteRoleHandler,
            GetRoleByIdHandler getRoleByIdHandler)
        {
            _logger = logger;
            _getAllRolesHandler = getAllRolesHandler;
            _getDataRolesHandler = getDataRolesHandler;
            _createRoleHandler = createRoleHandler;
            _updateRoleHandler = updateRoleHandler;
            _deleteRoleHandler = deleteRoleHandler;
            _getRoleByIdHandler = getRoleByIdHandler;
        }

        public async Task<IActionResult> Index(
            [FromQuery] RoleQueryViewModel query)
        {
            var search = new RoleSearch
            {
                Keyword = query.Keyword,
                Name = query.Name,
                SortBy = query.SortBy,
                SortDescending = query.SortDescending,
                Page = query.Page,
                PageSize = 10
            };

            var result = await _getDataRolesHandler.Handle(search, CancellationToken.None);
            return View(result);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateRoleCommand command)
        {
            try
            {
                if (!CurrentUserId.HasValue)
                {
                    ModelState.AddModelError("", "Người dùng chưa đăng nhập");
                    return View(command);
                }

                command.CreatedBy = CurrentUserId.Value;
                await _createRoleHandler.Handle(command, CancellationToken.None);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(command);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var role = await _getRoleByIdHandler.Handle(
                new GetRoleByIdQuery { Id = id }, 
                CancellationToken.None);
            
            if (role == null)
            {
                return NotFound();
            }
            
            // Map RoleDto to UpdateRoleCommand for the view
            var command = new UpdateRoleCommand
            {
                Id = role.Id,
                Name = role.Name,
                Description = role.Description
            };

            return View(command);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(UpdateRoleCommand command)
        {
            try
            {
                if (!CurrentUserId.HasValue)
                {
                    ModelState.AddModelError("", "Người dùng chưa đăng nhập");
                    return View(command);
                }

                command.UpdatedBy = CurrentUserId.Value;
                await _updateRoleHandler.Handle(command, CancellationToken.None);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(command);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                await _deleteRoleHandler.Handle(
                    new DeleteRoleCommand { Id = id }, 
                    CancellationToken.None);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi xóa role");
            }
            return RedirectToAction(nameof(Index));
        }
    }
}

