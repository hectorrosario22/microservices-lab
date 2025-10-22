using Auth.Api.DTOs;
using Auth.Api.Repositories;
using Auth.Api.Security;
using EasyNetQ;
using Events.Contracts;

namespace Auth.Api.Services;

internal sealed class UserService(
    IUserRepository repository,
    IPasswordHasher passwordHasher,
    IBus bus) : IUserService
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

        var message = new UserCreatedMessage(
            newUser.Id,
            newUser.Username,
            newUser.Email,
            $"{newUser.FirstName} {newUser.LastName}"
        );
        await bus.PubSub.PublishAsync(message, cancellationToken);
        return Result.Success();
    }
}