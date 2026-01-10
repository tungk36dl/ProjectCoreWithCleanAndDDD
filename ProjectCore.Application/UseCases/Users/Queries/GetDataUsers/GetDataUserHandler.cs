using ProjectCore.Application.Dtos.Users;
using ProjectCore.Application.Interfaces;
using ProjectCore.Application.Mappings;
using ProjectCore.Domain.Interfaces.UserRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectCore.Application.UseCases.Users.Queries.GetDataUsers
{
    public class GetDataUserHandler
    {
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        public GetDataUserHandler(IUserRepository userRepository, IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<List<UserDto>> Handle(UserSearch search, CancellationToken cancellationToken)
        {
            var users = await _userRepository.GetDataAsync(search, cancellationToken);
            var response = users.Select(user => UserMapper.ToDto(user)).ToList();
            return response;
        }


    }
}
