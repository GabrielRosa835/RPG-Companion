namespace Utils.UnionTypes;

public readonly struct Attempt<TSuccess> : IEquatable<Attempt<TSuccess>>
{
    private readonly TSuccess _successValue;
    private readonly Exception _failureValue;
    private readonly bool _isSuccess;

    public bool IsSuccess => _isSuccess;
    public bool IsFailure => !IsSuccess;

    private Attempt(TSuccess successValue, Exception failureValue, bool isSuccess)
    {
        _successValue = successValue;
        _failureValue = failureValue;
        _isSuccess = isSuccess;
    }

    public static Attempt<TSuccess> Success(TSuccess successValue) => new(successValue, default!, true);
    public static Attempt<TSuccess> Failure(Exception failureValue) => new(default!, failureValue, false);
    
    public static implicit operator Attempt<TSuccess>(TSuccess successValue) => Success(successValue);
    public static implicit operator Attempt<TSuccess>(Exception failureValue) => Failure(failureValue);
    public static implicit operator bool(Attempt<TSuccess> attempt) => attempt.IsSuccess;

    public TResult Either<TResult>(Func<TSuccess, TResult> onSuccess, Func<Exception, TResult> onFailure)
        => _isSuccess ? onSuccess(_successValue) : onFailure(_failureValue);

    public bool Equals(Attempt<TSuccess> other) => _isSuccess == other._isSuccess && (_isSuccess 
        ? EqualityComparer<TSuccess>.Default.Equals(_successValue, other._successValue)
        : EqualityComparer<Exception>.Default.Equals(_failureValue, other._failureValue));

    public override bool Equals(object? obj) => obj is Attempt<TSuccess> attempt && Equals(attempt);
    public override int GetHashCode() => HashCode.Combine(_successValue, _failureValue, _isSuccess);
}