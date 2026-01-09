using ProjectCore.Application.Dtos.Users;
using ProjectCore.Application.Mappings;
using ProjectCore.Domain.Interfaces;

namespace ProjectCore.Application.UseCases.Users.Queries.GetUserByUserNameOrEmail
{
    public class GetUserByUserNameEmailHandler
    {
         private readonly IUserRepository _userRepository;
        public GetUserByUserNameEmailHandler(IUserRepository userRepository)
        {
             _userRepository = userRepository;
        }
        public async Task<UserDto> Handle(
            GetUserByUserNameOrEmailQuery query,
            CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByUserNameOrEmailAsync(
                query.UserNameOrEmail,
                cancellationToken);
            if (user is null)
                throw new Exception("User not found");
            return UserMapper.ToDto(user);
        }
    }
}
