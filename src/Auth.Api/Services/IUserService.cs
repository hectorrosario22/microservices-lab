using Auth.Api.DTOs;

namespace Auth.Api.Services;

public interface IUserService
{
    Task<Result> RegisterUserAsync(UserRegistrationRequest request, CancellationToken cancellationToken);
}
