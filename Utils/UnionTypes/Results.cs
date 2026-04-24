namespace Utils.UnionTypes;

public static class Results
{
    public static Maybe<T> Some<T>(T value) => Maybe<T>.Some(value);
    public static Maybe<T> Empty<T>() => Maybe<T>.None();
    public static Maybe<T> Empty<T>(T unused) => Maybe<T>.None();

    public static Maybe<T> Perhaps<T>(T? value) => Maybe<T>.From(value);
    public static Maybe<T> Perhaps<T>(Func<T?> value) => Maybe<T>.From(value());

    public static Attempt<Unit, TF> Success<TF>() => Attempt<Unit, TF>.Success(Unit.Value);
    public static Attempt<TS, TF> Success<TS, TF>(TS sucessValue) => Attempt<TS, TF>.Success(sucessValue);
    public static Attempt<TS, TF> Failure<TS, TF>(TF failureValue) => Attempt<TS, TF>.Failure(failureValue);

    public static Attempt<TS> Success<TS>(TS sucessValue) => Attempt<TS>.Success(sucessValue);
    public static Attempt<TS> Failure<TS>(Exception e) => Attempt<TS>.Failure(e);
    public static Attempt<TS> Failure<TS>() => Attempt<TS>.Failure(new UnitException());

    public static Attempt Success() => Attempt.Success();
    public static Attempt Failure(Exception e) => Attempt.Failure(e);
    public static Attempt Failure() => Attempt.Failure(new UnitException());
}