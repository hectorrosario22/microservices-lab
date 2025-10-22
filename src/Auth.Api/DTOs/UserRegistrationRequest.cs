namespace Auth.Api.DTOs;

public record UserRegistrationRequest(
    string Username,
    string Password,
    string FirstName,
    string LastName,
    string Email
);
