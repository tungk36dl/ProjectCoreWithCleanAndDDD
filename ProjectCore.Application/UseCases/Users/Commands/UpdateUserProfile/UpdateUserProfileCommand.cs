using ProjectCore.Domain.ValueObjects.User;

namespace ProjectCore.Application.UseCases.Users.Commands.UpdateUserProfile;

public sealed class UpdateUserProfileCommand
{
    public Guid UserId { get; init; }

    public string? FullName { get; init; }
    public string? PhoneNumber { get; init; }
    public string? Address { get; init; }
    public string? AvatarUrl { get; init; }
    public DateOnly? DateOfBirth { get; init; }
    public Gender? Gender { get; init; }
    public Guid UpdatedBy { get; init; }
}
