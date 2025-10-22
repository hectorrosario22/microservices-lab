namespace Auth.Api.DTOs;

public record Result
{
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public string Error { get; }
    public ErrorCode ErrorCode { get; }

    protected Result(bool isSuccess, string error, ErrorCode errorCode)
    {
        if (isSuccess && error != null) throw new InvalidOperationException();
        if (!isSuccess && error == null) throw new InvalidOperationException();

        IsSuccess = isSuccess;
        Error = error!;
        ErrorCode = errorCode;
    }

    public static Result Success() => new(true, null!, ErrorCode.None);
    public static Result Failure(ErrorCode errorCode, string error) => new(false, error, errorCode);
}

public record Result<T> : Result
{
    private readonly T _value;
    public T Value => IsSuccess
        ? _value
        : throw new InvalidOperationException("Cannot access value on a failed result.");

    private Result(T value, bool isSuccess, string error, ErrorCode errorCode)
        : base(isSuccess, error, errorCode)
    {
        _value = value;
    }

    public static Result<T> Success(T value) => new(value, true, null!, ErrorCode.None);
    public static new Result<T> Failure(ErrorCode errorCode, string error)
        => new(default!, false, error, errorCode);
}
