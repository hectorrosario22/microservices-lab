using Auth.Api.DTOs;
using Auth.Api.Repositories;
using Auth.Api.Security;

namespace Auth.Api.Services;

internal sealed class UserService(
    IUserRepository repository,
    IPasswordHasher passwordHasher) : IUserService
{
    public async Task<Result> RegisterUserAsync(
        UserRegistrationRequest request,
        CancellationToken cancellationToken)
    {
        var isUniqueUsername = !await repository.ExistsByUsernameAsync(request.Username, cancellationToken);
        if (!isUniqueUsername)
        {
            return Result.Failure(ErrorCode.Conflict, "Username is already taken.");
        }

        var newUser = new Entities.User
        {
            Username = request.Username,
            PasswordHash = passwordHasher.HashPassword(request.Password),
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
        };
        await repository.CreateUserAsync(newUser, cancellationToken);
        return Result.Success();
    }
}