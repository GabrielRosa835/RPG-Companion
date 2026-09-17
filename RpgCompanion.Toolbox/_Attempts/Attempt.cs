namespace RpgCompanion.Toolbox;

public readonly record struct Attempt
{
    private readonly Exception _failureValue;
    private readonly bool _isSuccess;

    public bool IsSuccess => _isSuccess;

    public bool IsFailure => !IsSuccess;

    private Attempt(Exception failureValue, bool isSuccess)
    {
        _failureValue = failureValue;
        _isSuccess = isSuccess;
    }

    public static Attempt Success() => new(default!, true);

    public static Attempt Failure(Exception failureValue) => new(failureValue, false);

    public static implicit operator Attempt(Exception failureValue) => Failure(failureValue);

    public static implicit operator bool(Attempt attempt) => attempt.IsSuccess;

    public TResult Either<TResult>(Func<TResult> onSuccess, Func<Exception, TResult> onFailure)
        => _isSuccess ? onSuccess() : onFailure(_failureValue);
}

public readonly record struct Attempt<TSuccess>
{
    private readonly TSuccess _successValue;
    private readonly Exception _failureValue;
    private readonly bool _isSuccess;

    public bool IsSuccess => _isSuccess;
    public bool IsFailure => !IsSuccess;

    private Attempt(Exception failureValue, TSuccess successValue, bool isSuccess)
    {
        _successValue = successValue;
        _failureValue = failureValue;
        _isSuccess = isSuccess;
    }

    public static Attempt<TSuccess> Failure(Exception failureValue) => new(failureValue, default!, false);
    public static Attempt<TSuccess> Success(TSuccess successValue) => new(default!, successValue, true);

    public static implicit operator Attempt<TSuccess>(Exception failureValue) => Failure(failureValue);
    public static implicit operator Attempt<TSuccess>(TSuccess successValue) => Success(successValue);

    public static implicit operator bool(Attempt<TSuccess> attempt) => attempt.IsSuccess;

    public TResult Either<TResult>(Func<TSuccess, TResult> onSuccess, Func<Exception, TResult> onFailure)
        => _isSuccess ? onSuccess(_successValue) : onFailure(_failureValue);
}

public readonly record struct Attempt<TSuccess, TFailure>
{
    private readonly TSuccess _successValue;
    private readonly TFailure _failureValue;
    private readonly bool _isSuccess;

    public bool IsSuccess => _isSuccess;
    public bool IsFailure => !IsSuccess;

    private Attempt(TFailure failureValue, TSuccess successValue, bool isSuccess)
    {
        _successValue = successValue;
        _failureValue = failureValue;
        _isSuccess = isSuccess;
    }

    public static Attempt<TSuccess, TFailure> Failure(TFailure failureValue) => new(failureValue, default!, false);
    public static Attempt<TSuccess, TFailure> Success(TSuccess successValue) => new(default!, successValue, true);

    public static implicit operator Attempt<TSuccess, TFailure>(TFailure failureValue) => Failure(failureValue);
    public static implicit operator Attempt<TSuccess, TFailure>(TSuccess successValue) => Success(successValue);

    public static implicit operator bool(Attempt<TSuccess, TFailure> attempt) => attempt.IsSuccess;

    public TResult Either<TResult>(Func<TSuccess, TResult> onSuccess, Func<TFailure, TResult> onFailure)
        => _isSuccess ? onSuccess(_successValue) : onFailure(_failureValue);
}
