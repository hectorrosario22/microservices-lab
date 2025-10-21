using Auth.Api.Entities;
using Auth.Api.Repositories;
using Auth.Api.Requests;
using Auth.Api.Security;

namespace Auth.Api.Endpoints;

public static class UserEndpoints
{
    public static IEndpointRouteBuilder MapUserEndpoints(this IEndpointRouteBuilder endpoint)
    {
        endpoint.MapPost("/users", async (
            UserRegistrationRequest request,
            IUserRepository repository,
            IPasswordHasher passwordHasher) =>
        {
            // TODO: Validate request
            var newUser = new User
            {
                Username = request.Username,
                PasswordHash = passwordHasher.HashPassword(request.Password),
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
            };
            await repository.CreateUserAsync(newUser);
            return Results.NoContent();
        }).WithName("RegisterUser");

        return endpoint;
    }
}