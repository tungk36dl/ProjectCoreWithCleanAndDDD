using Microsoft.Extensions.DependencyInjection;
using ProjectCore.Application.UseCases.Users.Commands.CreateUser;
using ProjectCore.Application.UseCases.Users.Commands.UpdateUserProfile;
using ProjectCore.Application.UseCases.Users.Queries.GetUserById;
using ProjectCore.Application.UseCases.Users.Queries.GetUserByUserNameOrEmail;
using ProjectCore.Application.UseCases.Users.Queries.GetAllUsers;
using ProjectCore.Application.UseCases.Users.Commands.UpdateUser;
using ProjectCore.Application.UseCases.Users.Commands.DeleteUser;
using ProjectCore.Application.UseCases.Users.Queries.GetDataUsers;
using ProjectCore.Application.UseCases.Roles.Queries.GetAllRoles;
using ProjectCore.Application.UseCases.Roles.Commands.CreateRole;
using ProjectCore.Application.UseCases.Roles.Commands.UpdateRole;
using ProjectCore.Application.UseCases.Roles.Commands.DeleteRole;
using ProjectCore.Application.UseCases.Roles.Queries.GetRoleById;
using ProjectCore.Application.UseCases.Roles.Queries.GetDataRoles;
using ProjectCore.Application.UseCases.Permissions.Queries.GetAllPermissions;
using ProjectCore.Application.UseCases.SeedData;

namespace ProjectCore.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            // User handlers
            services.AddScoped<LoginUserHandler>();
            services.AddScoped<CreateUserHandler>();
            services.AddScoped<UpdateUserProfileHandler>();
            services.AddScoped<GetUserByIdHandler>();
            services.AddScoped<GetUserByUserNameEmailHandler>();
            services.AddScoped<GetAllUsersHandler>();
            services.AddScoped<UpdateUserHandler>();
            services.AddScoped<DeleteUserHandler>();
            services.AddScoped<GetDataUserHandler>();
            
            // Role handlers
            services.AddScoped<GetAllRolesHandler>();
            services.AddScoped<GetDataRolesHandler>();
            services.AddScoped<GetRoleByIdHandler>();
            services.AddScoped<CreateRoleHandler>();
            services.AddScoped<UpdateRoleHandler>();
            services.AddScoped<DeleteRoleHandler>();

            // Permission handlers
            services.AddScoped<GetAllPermissionsHandler>();

            // Seed data handler
            services.AddScoped<SeedDataHandler>();

            return services;
        }
    }
}
