using ProjectCore.Application.Common.Security;
using ProjectCore.Application.Interfaces;
using ProjectCore.Application.UseCases.Users.Commands.Login;
using ProjectCore.Domain.Exceptions;
using ProjectCore.Domain.Interfaces;

public sealed class LoginUserHandler
{
    private readonly IUserRepository _userRepository;
    private readonly IPermissionQueryRepository _permissionQueryRepository;
    private readonly IPasswordHasher _passwordHasher;

    public LoginUserHandler(
        IUserRepository userRepository,
        IPermissionQueryRepository permissionQueryRepository,
        IPasswordHasher passwordHasher)
    {
        _userRepository = userRepository;
        _permissionQueryRepository = permissionQueryRepository;
        _passwordHasher = passwordHasher;
    }

    public async Task<LoginUserResult> Handle(
        LoginUserCommand command,
        CancellationToken cancellationToken)
    {
        var user = await _userRepository
            .GetByUserNameOrEmailAsync(command.UserNameOrEmail, cancellationToken);

        if (user is null)
            throw new InvalidLoginException();

        if (!_passwordHasher.Verify(
                user.PasswordHash,
                command.Password))
            throw new InvalidLoginException();

        var permissions = await _permissionQueryRepository
            .GetPermissionsByUserIdAsync(user.Id, cancellationToken);

        return new LoginUserResult
        {
            UserId = user.Id,
            UserName = user.UserName.ToString(),
            Email = user.Email.ToString(),
            Permissions = permissions.Select(p => p.ToString()).ToList()
        };
    }
}
