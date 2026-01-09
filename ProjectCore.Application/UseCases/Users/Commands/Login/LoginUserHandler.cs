using ProjectCore.Application.Common.Security;
using ProjectCore.Domain.Entities;
using ProjectCore.Domain.Exceptions;
using ProjectCore.Domain.Interfaces;
using ProjectCore.Domain.ValueObjects.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectCore.Application.UseCases.Users.Commands.Login
{
    public class LoginUserHandler
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;
        public LoginUserHandler(IUserRepository userRepository, IPasswordHasher passwordHasher)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
        }
        public async Task<LoginUserResult> Handle(
            LoginUserCommand command,
            CancellationToken cancellationToken)
        {
            // Implementation for user login goes here.
            User? user;
            if(Email.IsValid(command.UserNameOrEmail))
            {
                user = _userRepository.GetByEmailAsync( new Email(command.UserNameOrEmail),
                    cancellationToken).Result;
            }
            else
            {
                user = await _userRepository.GetByUserNameAsync( new UserName(command.UserNameOrEmail),
                    cancellationToken);
            }

            if(user == null || _passwordHasher.Verify(user.PasswordHash, command.Password))
            {
                throw new InvalidLoginException();
            }
            return new LoginUserResult
            {
                UserId = user.Id,
                UserName = user.UserName.ToString(),
                Roles = user.UserRoles.Select(ur => ur.RoleId.ToString()).ToList()
            };
        }
    }
}
