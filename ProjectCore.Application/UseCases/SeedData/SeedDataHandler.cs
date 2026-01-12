using Microsoft.Extensions.Logging;
using ProjectCore.Application.Common.Configuration;
using ProjectCore.Application.Common.Security;
using ProjectCore.Application.Interfaces;
using ProjectCore.Application.UseCases.Permissions.Scan;
using ProjectCore.Domain.Entities;
using ProjectCore.Domain.Interfaces.PermissionRepository;
using ProjectCore.Domain.Interfaces.RoleRepository;
using ProjectCore.Domain.Interfaces.UserRepository;
using ProjectCore.Domain.ValueObjects.Role;
using ProjectCore.Domain.ValueObjects.User;


namespace ProjectCore.Application.UseCases.SeedData;
public sealed class SeedDataHandler
{
    private readonly ILogger<SeedDataHandler> _logger;
    private readonly IUserRepository _userRepository;
    private readonly IRoleRepository _roleRepository;
    private readonly IPermissionRepository _permissionRepository;
    private readonly IPermissionScanner _permissionScanner;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAdminSeedConfig _adminSeedConfig;
    private readonly IAdminRoleSeedConfig _adminRoleSeedConfig;

    public SeedDataHandler(
        ILogger<SeedDataHandler> logger,
        IUserRepository userRepository,
        IRoleRepository roleRepository,
        IPermissionRepository permissionRepository,
        IPermissionScanner permissionScanner,
        IPasswordHasher passwordHasher,
        IUnitOfWork unitOfWork,
        IAdminSeedConfig adminSeedConfig,
        IAdminRoleSeedConfig adminRoleSeedConfig)
    {
        _logger = logger;
        _userRepository = userRepository;
        _roleRepository = roleRepository;
        _permissionRepository = permissionRepository;
        _permissionScanner = permissionScanner;
        _passwordHasher = passwordHasher;
        _unitOfWork = unitOfWork;
        _adminSeedConfig = adminSeedConfig;
        _adminRoleSeedConfig = adminRoleSeedConfig;
    }

    public async Task SeedAsync(CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("=== System Bootstrap Started ===");

        await _unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            var systemUserId = SystemAccount.Id;

            // 1️⃣ Sync permissions
            await SyncPermissionsAsync(systemUserId, cancellationToken);

            // 2️⃣ Ensure admin role
            var adminRole = await EnsureAdminRoleAsync(systemUserId, cancellationToken);

            // 3️⃣ Ensure admin has all permissions
            await EnsureAdminHasAllPermissionsAsync(adminRole, systemUserId, cancellationToken);

            // 4️⃣ Ensure admin user
            await EnsureAdminUserAsync(adminRole.Id, systemUserId, cancellationToken);

            await _unitOfWork.CommitAsync(cancellationToken);

            _logger.LogInformation("=== System Bootstrap Completed ===");
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackAsync(cancellationToken);
            _logger.LogError(ex, "System bootstrap failed – rollback executed");
            throw;
        }
    }


    // ========================== PERMISSIONS ==========================

    private async Task SyncPermissionsAsync(Guid systemUserId, CancellationToken ct)
    {
        var scanned = _permissionScanner.Scan();

        foreach (var p in scanned)
        {
            if (await _permissionRepository.ExistsAsync(p.Module, p.Action, ct))
                continue;

            await _permissionRepository.AddAsync(
                new Permission(Guid.NewGuid(), p.Module, p.Action, systemUserId),
                ct);
        }
    }

    // ========================== ROLE ==========================

    private async Task<Role> EnsureAdminRoleAsync(Guid systemUserId, CancellationToken ct)
    {
        var roleName = new RoleName(_adminRoleSeedConfig.Name);

        var role = await _roleRepository.GetByNameAsync(roleName, ct);
        if (role != null) return role;

        role = new Role(
            Guid.NewGuid(),
            roleName,
            _adminRoleSeedConfig.Description ?? "System administrator",
            systemUserId);

        await _roleRepository.AddAsync(role, ct);
        return role;
    }

    // ========================== ROLE → PERMISSION ==========================

    private async Task EnsureAdminHasAllPermissionsAsync(
        Role adminRole,
        Guid systemUserId,
        CancellationToken ct)
    {
        var allPermissions = await _permissionRepository.GetAllAsync(ct);

        foreach (var p in allPermissions)
        {
            adminRole.AddPermission(p.Id, systemUserId);
        }

        _roleRepository.Update(adminRole);
    }

    // ========================== USER ==========================

    private async Task EnsureAdminUserAsync(Guid adminRoleId, Guid systemUserId, CancellationToken ct)
    {
        var userName = new UserName(_adminSeedConfig.UserName);

        var existing = await _userRepository.GetByUserNameAsync(userName, ct);
        if (existing != null)
        {
            // Ensure user has admin role
            //existing.AssignRole(adminRoleId, systemUserId);
            //_userRepository.Update(existing);
            return;
        }

        var hash = _passwordHasher.Hash(_adminSeedConfig.Password);

        var admin = new User(
            Guid.NewGuid(),
            userName,
            new Email(_adminSeedConfig.Email),
            hash,
            systemUserId);

        if (!string.IsNullOrWhiteSpace(_adminSeedConfig.FullName))
        {
            admin.UpdateProfile(
                new FullName(_adminSeedConfig.FullName),
                null, null, null, null, null,
                systemUserId);
        }

        admin.AssignRole(adminRoleId, systemUserId);

        await _userRepository.AddAsync(admin, ct);
    }
}
