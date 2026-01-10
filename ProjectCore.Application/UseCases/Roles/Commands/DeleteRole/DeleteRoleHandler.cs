using ProjectCore.Application.Interfaces;
using ProjectCore.Domain.Exceptions;
using ProjectCore.Domain.Interfaces.RoleRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectCore.Application.UseCases.Roles.Commands.DeleteRole
{
    public class DeleteRoleHandler
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IUnitOfWork _unitOfWork;
        public DeleteRoleHandler(IRoleRepository roleRepository, IUnitOfWork unitOfWork)
        {
            _roleRepository = roleRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task Handle(DeleteRoleCommand command, CancellationToken cancellationToken)
        {
            var role = await _roleRepository.GetByIdAsync(command.Id, cancellationToken);
            if (role == null)
            {
                throw new RoleNotFoundException(command.Id);
            }
            _roleRepository.Remove(role);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
