using ProjectCore.Application.Interfaces;
using ProjectCore.Domain.Interfaces;
using ProjectCore.Domain.ValueObjects.User;

namespace ProjectCore.Application.UseCases.Users.Commands.UpdateUserProfile
{
    public sealed class UpdateUserProfileHandler
    {
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateUserProfileHandler(
            IUserRepository userRepository,
            IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(
            UpdateUserProfileCommand command,
            CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(command.UserId, cancellationToken);
            if (user == null)
            {
                throw new Exception($"User with ID {command.UserId} not found.");
            }
            user.UpdateProfile(
                new FullName(command.FullName), 
                new PhoneNumber(command.PhoneNumber),
                command.Gender, 
                command.DateOfBirth,
                command.Address, 
                Avatar.FromUrl(command.AvatarUrl),
                command.UpdatedBy);
            _userRepository.Update(user);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
