using ProjectCore.Application.Interfaces;
using ProjectCore.Domain.Exceptions;
using ProjectCore.Domain.Interfaces.RoleRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectCore.Application.UseCases.Roles.Commands.UpdateRole
{
    public class UpdateRoleHandler
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IUnitOfWork _unitOfWork;
        public UpdateRoleHandler(IRoleRepository roleRepository, IUnitOfWork unitOfWork)
        {
            _roleRepository = roleRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task Handle(UpdateRoleCommand command, CancellationToken cancellationToken)
        {
            var role = await _roleRepository.GetByIdAsync(command.Id, cancellationToken);
            if (role == null)
            {
                throw new RoleNotFoundException(command.Id);
            }
            role.UpdateDetails(
                command.Name,
                command.Description,
                command.UpdatedBy
            );
            _roleRepository.Update(role);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
