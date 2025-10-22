namespace Events.Contracts;

public record UserCreatedMessage(
    string UserId,
    string Username,
    string Email,
    string FullName);
