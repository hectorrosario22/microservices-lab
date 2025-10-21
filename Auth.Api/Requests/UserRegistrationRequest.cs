namespace Auth.Api.Requests;

public record UserRegistrationRequest(
    string Username,
    string Password,
    string FirstName,
    string LastName,
    string Email
);