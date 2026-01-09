using Microsoft.AspNetCore.Mvc;
using ProjectCore.Application.Common.Security;
using ProjectCore.Domain.Interfaces;

namespace ProjectCore.Presentation.MVC.Controllers
{
    public class UserController : BaseController
    {
        private readonly ILogger<UserController> _logger;
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;


        public UserController(ILogger<UserController> logger, IPasswordHasher passwordHasher, IUserRepository userRepository)
        {
            _logger = logger;
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
        }

        public IActionResult Index()
        {
            return View();
        }

    }
}
