namespace RpgCompanion.Canva;

public readonly record struct Actor<T>
{
    private readonly T _current;
    public T Current => _current;

    public Actor(T value) => _current = value;

    public Actor<T> Apply(Rule<T> rule) => new(rule(_current));
    public Actor<TU> Apply<TU>(Effect<T, TU> effect) => new(effect(_current));
}

public static class Actor
{
    public static Actor<T> Of<T>(T value) => new(value);
    public static Actor<T> AsActor<T>(this T value) => value is Actor<T> actor ? actor : new(value);
}
