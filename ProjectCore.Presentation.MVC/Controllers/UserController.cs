using Microsoft.AspNetCore.Mvc;
using ProjectCore.Application.Common.Security;
using ProjectCore.Application.UseCases.Users.Commands.CreateUser;
using ProjectCore.Application.UseCases.Users.Commands.DeleteUser;
using ProjectCore.Application.UseCases.Users.Commands.UpdateUser;
using ProjectCore.Application.UseCases.Users.Queries.GetAllUsers;
using ProjectCore.Application.UseCases.Users.Queries.GetUserById;

namespace ProjectCore.Presentation.MVC.Controllers
{
    public class UserController : BaseController
    {
        private readonly ILogger<UserController> _logger;
        private readonly IPasswordHasher _passwordHasher;


        private readonly GetAllUsersHandler _getAllUsersHandler;
        private readonly CreateUserHandler _createUserHandler;
        private readonly UpdateUserHandler _updateUserHandler;
        private readonly DeleteUserHandler _deleteUserHandler;
        private readonly GetUserByIdHandler _getUserByIdHandler;

        public UserController(
            ILogger<UserController> logger,
            IPasswordHasher passwordHasher,
            GetAllUsersHandler getAllUsersHandler,
            CreateUserHandler createUserHandler,
            UpdateUserHandler updateUserHandler,
            DeleteUserHandler deleteUserHandler,
            GetUserByIdHandler getUserByIdHandler)
        {
            _logger = logger;
            _passwordHasher = passwordHasher;
            _getAllUsersHandler = getAllUsersHandler;
            _createUserHandler = createUserHandler;
            _updateUserHandler = updateUserHandler;
            _deleteUserHandler = deleteUserHandler;
            _getUserByIdHandler = getUserByIdHandler;
        }

        public async Task<IActionResult> Index()
        {
            var users = await _getAllUsersHandler.Handle(new GetAllUsersQuery(), CancellationToken.None);
            return View(users);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateUserCommand command)
        {
            try
            {
                await _createUserHandler.Handle(command, CancellationToken.None);
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
            var user = await _getUserByIdHandler.Handle(new GetUserByIdQuery { UserId = id }, CancellationToken.None);
            if (user == null)
            {
                return NotFound();
            }
            
            // Map UserDto to UpdateUserCommand for the view
            var command = new UpdateUserCommand
            {
                Id = user.Id,
                FullName = user.FullName,
                PhoneNumber = user.PhoneNumber,
                Gender = user.Gender,
                DateOfBirth = user.DateOfBirth,
                Address = user.Address,
                AvatarUrl = user.AvatarUrl
            };

            return View(command);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(UpdateUserCommand command)
        {
             try
            {
                await _updateUserHandler.Handle(command, CancellationToken.None);
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
                await _deleteUserHandler.Handle(new DeleteUserCommand { Id = id }, CancellationToken.None);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting user");
            }
            return RedirectToAction(nameof(Index));
        }

    }
}
