using ProjectCore.Application.Interfaces;
using ProjectCore.Domain.Exceptions;
using ProjectCore.Domain.Interfaces.UserRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectCore.Application.UseCases.Users.Commands.DeleteUser
{
    public sealed class DeleteUserHandler
    {
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteUserHandler(IUserRepository userRepository, IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(DeleteUserCommand command, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(command.Id, cancellationToken);
            if (user == null)
            {
                throw new UserNotFoundException();
            }

            _userRepository.Remove(user);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
