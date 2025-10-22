namespace Auth.Api.Security;

public enum PasswordVerificationResult
{
    Failed,
    Success,
    SuccessRehashNeeded
}
