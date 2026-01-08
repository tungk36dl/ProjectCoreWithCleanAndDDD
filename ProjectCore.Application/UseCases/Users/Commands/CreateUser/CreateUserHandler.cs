using ProjectCore.Application.Interfaces;
using ProjectCore.Domain.Entities;
using ProjectCore.Domain.Exceptions;
using ProjectCore.Domain.Interfaces;
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
        private readonly IUnitOfWork _unitOfWork;

        public CreateUserHandler(
            IUserRepository userRepository,
            IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
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
                command.PasswordHash,
                command.CreatedBy);

            await _userRepository.AddAsync(user, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return user.Id;
        }
    }

}
