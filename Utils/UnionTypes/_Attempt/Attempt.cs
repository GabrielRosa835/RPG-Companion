namespace Utils.UnionTypes;

public readonly struct Attempt : IEquatable<Attempt>
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

    public bool Equals(Attempt other)
    {
        return _isSuccess == other._isSuccess &&
               (_isSuccess || _failureValue.Equals(other._failureValue));
    }
}