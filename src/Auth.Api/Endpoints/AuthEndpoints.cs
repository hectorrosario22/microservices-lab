using Auth.Api.DTOs;
using Auth.Api.Services;
using SharpGrip.FluentValidation.AutoValidation.Endpoints.Extensions;

namespace Auth.Api.Endpoints;

public static class AuthEndpoints
{
    public static IEndpointRouteBuilder MapAuthEndpoints(this IEndpointRouteBuilder endpoint)
    {
        endpoint.MapPost("/auth/register", async (
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
        })
        .WithName("RegisterUser")
        .AddFluentValidationAutoValidation();

        return endpoint;
    }
}