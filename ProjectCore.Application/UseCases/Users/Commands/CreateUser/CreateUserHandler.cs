using ProjectCore.Application.Interfaces;
using ProjectCore.Domain.Entities;
using ProjectCore.Domain.Exceptions;
using ProjectCore.Domain.Interfaces.UserRepository;
using ProjectCore.Domain.Interfaces.RoleRepository;
using ProjectCore.Domain.ValueObjects.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectCore.Application.UseCases.Users.Commands.CreateUser
{
    public sealed class CreateUserHandler
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateUserHandler(
            IUserRepository userRepository,
            IRoleRepository roleRepository,
            IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Guid> Handle(
            CreateUserCommand command,
            CancellationToken cancellationToken)
        {
            if (await _userRepository.ExistsByEmailAsync(
                    new Email(command.Email), cancellationToken))
                throw new UserEmailAlreadyExistsException(command.Email);
            if (await _userRepository.ExistsByUserNameAsync(
                    new UserName(command.UserName), cancellationToken))
                throw new UserNameAlreadyExistsException(command.UserName);

            var user = new User(
                Guid.NewGuid(),
                new UserName(command.UserName),
                new Email(command.Email),
                command.PasswordHash);

            // Gán roles cho user
            if (command.RoleIds != null && command.RoleIds.Any())
            {
                var createdBy = command.CreatedBy ?? Guid.Empty;
                foreach (var roleId in command.RoleIds)
                {
                    // Validate role tồn tại
                    var role = await _roleRepository.GetByIdAsync(roleId, cancellationToken);
                    if (role == null)
                    {
                        throw new Exception($"Role với ID {roleId} không tồn tại.");
                    }
                    
                    user.AssignRole(roleId, createdBy);
                }
            }

            await _userRepository.AddAsync(user, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return user.Id;
        }
    }

}
