namespace RpgCompanion.Core.Toolbox;

public static class Results
{
    public static Maybe<T> Some<T>(T value) => Maybe<T>.Some(value);
    public static Maybe<T> Empty<T>() => Maybe<T>.None();
    public static Maybe<T> Empty<T>(T unused) => Maybe<T>.None();

    public static Maybe<T> Perhaps<T>(T? value) => Maybe<T>.From(value);
    public static Maybe<T> Perhaps<T>(Func<T?> value) => Maybe<T>.From(value());

    public static Attempt<None, TF> Success<TF>() => Attempt<None, TF>.Success(None.Value);
    public static Attempt<TS, TF> Success<TS, TF>(TS successValue) => Attempt<TS, TF>.Success(successValue);
    public static Attempt<TS, TF> Failure<TS, TF>(TF failureValue) => Attempt<TS, TF>.Failure(failureValue);

    public static Attempt<TS> Success<TS>(TS successValue) => Attempt<TS>.Success(successValue);
    public static Attempt<TS> Failure<TS>(Exception e) => Attempt<TS>.Failure(e);
    public static Attempt<TS> Failure<TS>() => Attempt<TS>.Failure(new NoneException());

    public static Attempt Success() => Attempt.Success();
    public static Attempt Failure(Exception e) => Attempt.Failure(e);
    public static Attempt Failure() => Attempt.Failure(new NoneException());
}
