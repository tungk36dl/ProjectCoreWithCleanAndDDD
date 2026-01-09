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

namespace ProjectCore.Application.UseCases.Users.Commands.UpdateUser
{
    public sealed class UpdateUserHandler
    {
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateUserHandler(IUserRepository userRepository, IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(UpdateUserCommand command, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(command.Id, cancellationToken);
            if (user == null)
            {
                throw new UserNotFoundException();
            }

            Gender? gender = null;
            if (!string.IsNullOrEmpty(command.Gender))
            {
                if (Enum.TryParse<Gender>(command.Gender, out var g))
                {
                    gender = g;
                }
            }

            // Using standard domain types as per UpdateProfile signature
            // public void UpdateProfile(FullName? fullName, PhoneNumber? phoneNumber, Gender? gender, DateOnly? dateOfBirth, string? address, Avatar? avatar, Guid updatedBy)
            
            user.UpdateProfile(
                command.FullName != null ? new FullName(command.FullName) : null,
                command.PhoneNumber != null ? new PhoneNumber(command.PhoneNumber) : null,
                gender,
                command.DateOfBirth,
                command.Address,
                command.AvatarUrl != null ? Avatar.FromUrl(command.AvatarUrl) : null,
                command.UpdatedBy
            );

            _userRepository.Update(user);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
