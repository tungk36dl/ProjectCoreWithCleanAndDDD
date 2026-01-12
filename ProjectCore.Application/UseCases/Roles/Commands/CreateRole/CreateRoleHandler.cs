using ProjectCore.Application.Interfaces;
using ProjectCore.Domain.Entities;
using ProjectCore.Domain.Exceptions;
using ProjectCore.Domain.Interfaces.RoleRepository;
using ProjectCore.Domain.ValueObjects.Role;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectCore.Application.UseCases.Roles.Commands.CreateRole
{
    public class CreateRoleHandler
    {
        private readonly IRoleRepository _roleRepository;
        public readonly IUnitOfWork _unitOfWork;
        public CreateRoleHandler(
            IRoleRepository roleRepository,
            IUnitOfWork unitOfWork)
        {
            _roleRepository = roleRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Guid> Handle(
            CreateRoleCommand command,
            CancellationToken cancellationToken)
        {
            if (await _roleRepository.ExistsByNameAsync(new RoleName(command.RoleName), cancellationToken))
            {
                throw new RoleNameAlreadyExistsException($"Role with name '{command.RoleName}' already exists.");
            }

            var role = new Role(
                Guid.NewGuid(),
                new RoleName(command.RoleName),
                command.Description,
                command.CreatedBy);

            // Gán permissions cho role
            if (command.PermissionIds != null && command.PermissionIds.Any())
            {
                foreach (var permissionId in command.PermissionIds)
                {
                    role.AddPermission(permissionId, command.CreatedBy);
                }
            }

            await _roleRepository.AddAsync(role, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return role.Id;

        }
    }
}
