namespace Auth.Api.DTOs;

public enum ErrorCode
{
    None = 0,
    NotFound = StatusCodes.Status404NotFound,
    Conflict = StatusCodes.Status409Conflict,
    ValidationError = StatusCodes.Status400BadRequest,
}
