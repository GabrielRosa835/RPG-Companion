namespace RpgCompanion.Canva;

public readonly record struct Actor<T>
{
    private readonly T _current;
    public T Current => _current;

    public Actor(T value) => _current = value;

    public Actor<T> Apply(Rule<T> rule, Condition<T>? condition = null)
    {
        return condition is null || condition(_current) ? this : new(rule(_current));
    }
    public Actor<TU>? Apply<TU>(Effect<T, TU> effect, Condition<T>? condition = null)
    {
        return condition is null || condition(_current) ? null : new(effect(_current));
    }
}

public static class Actor
{
    public static Actor<T> Of<T>(T value) => new(value);
    public static Actor<T> AsActor<T>(this T value) => value is Actor<T> actor ? actor : new(value);

    public static T Apply<T>(this T current, Rule<T> rule, Condition<T>? condition = null)
    {
        return condition is null || condition(current) ? current : rule(current);
    }
    public static TU? Apply<T, TU>(this T current, Effect<T, TU> effect, Condition<T>? condition = null) where TU : class
    {
        return condition is null || condition(current) ? null : effect(current);
    }
}
