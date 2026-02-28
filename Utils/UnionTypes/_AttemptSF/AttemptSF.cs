namespace Utils.UnionTypes;

public readonly struct Attempt<TSuccess, TFailure> : IEquatable<Attempt<TSuccess, TFailure>>
{
    private readonly TSuccess _successValue;
    private readonly TFailure _failureValue;
    private readonly bool _isSuccess;

    public bool IsSuccess => _isSuccess;
    public bool IsFailure => !IsSuccess;
    
    private Attempt(TSuccess successValue, TFailure failureValue, bool isSuccess)
    {
        _successValue = successValue;
        _failureValue = failureValue;
        _isSuccess = isSuccess;
    }

    public static Attempt<TSuccess, TFailure> Success(TSuccess successValue) => new(successValue, default!, true);
    public static Attempt<TSuccess, TFailure> Failure(TFailure failureValue) => new(default!, failureValue, false);

    public static implicit operator Attempt<TSuccess, TFailure>(TSuccess successValue) => Success(successValue);
    public static implicit operator Attempt<TSuccess, TFailure>(TFailure failureValue) => Failure(failureValue);
    public static implicit operator bool(Attempt<TSuccess, TFailure> attempt) => attempt.IsSuccess;

    public TResult Either<TResult>(Func<TSuccess, TResult> onSuccess, Func<TFailure, TResult> onFailure)
        => _isSuccess ? onSuccess(_successValue) : onFailure(_failureValue);

    public bool Equals(Attempt<TSuccess, TFailure> other) => other._isSuccess == _isSuccess && (_isSuccess
        ? EqualityComparer<TSuccess>.Default.Equals(_successValue, other._successValue)
        : EqualityComparer<TFailure>.Default.Equals(_failureValue, other._failureValue));

    public override bool Equals(object? obj) => obj is Attempt<TSuccess, TFailure> attempt && Equals(attempt);
    public override int GetHashCode() => HashCode.Combine(_successValue, _failureValue, _isSuccess);
}