using ProjectCore.Application.Dtos.Users;
using ProjectCore.Application.Mappings;
using ProjectCore.Domain.Exceptions;
using ProjectCore.Domain.Interfaces.UserRepository;

namespace ProjectCore.Application.UseCases.Users.Queries.GetUserById
{
    public class GetUserByIdHandler
    {
        private readonly IUserRepository _userRepository;
        public GetUserByIdHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task<UserDto> Handle(
            GetUserByIdQuery query,
            CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(
                query.UserId,
                cancellationToken);
            if (user is null)
                throw new UserNotFoundException();
            return UserMapper.ToDto(user);
        }

    }
}
