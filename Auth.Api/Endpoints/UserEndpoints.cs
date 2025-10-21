using Auth.Api.DTOs;
using Auth.Api.Services;

namespace Auth.Api.Endpoints;

public static class UserEndpoints
{
    public static IEndpointRouteBuilder MapUserEndpoints(this IEndpointRouteBuilder endpoint)
    {
        endpoint.MapPost("/users", async (
            UserRegistrationRequest request,
            IUserService userService,
            CancellationToken cancellationToken) =>
        {
            var result = await userService.RegisterUserAsync(request, cancellationToken);
            if (result.IsFailure)
            {
                return result.ErrorCode switch
                {
                    ErrorCode.Conflict => Results.Conflict(new
                    {
                        Message = result.Error
                    }),
                    _ => Results.BadRequest(new
                    {
                        Message = result.Error
                    })
                };
            }

            return Results.NoContent();
        }).WithName("RegisterUser");

        return endpoint;
    }
}