namespace RpgCompanion.Core;

public static class Rule
{
    public static void Example()
    {
        IRule<int> checkAttack = default!;
        IRule<int> attack = default!;
        IRule<int, string> getMonsterName = default!;
        IRule<string, int> calculateVulnerabilities = default!;
        IRule<int> calculateFinalDamage = default!;
        IRule<int> action = attack & checkAttack | getMonsterName + calculateVulnerabilities | calculateFinalDamage;
    }

    public static IRule<TSubject> Create<TSubject>(Func<TSubject, IRuleContext, TSubject> handler)
        => new Rule<TSubject>(handler);

    public static IRule<TSubject, TResult> Create<TSubject, TResult>(Func<TSubject, IRuleContext, TResult> handler)
        => new Rule<TSubject, TResult>(handler);

    public static IAsyncRule<TSubject> Create<TSubject>(Func<TSubject, IRuleContext, Task<TSubject>> handler)
        => new AsyncRule<TSubject>(handler);

    public static IAsyncRule<TSubject, TResult> Create<TSubject, TResult>(Func<TSubject, IRuleContext, Task<TResult>> handler)
        => new AsyncRule<TSubject, TResult>(handler);

    public static IRule<T> Then<T>(this IRule<T> previous, IRule<T> next)
        => new Rule<T>((s, ctx)
            => next.Apply(previous.Apply(s, ctx), ctx));

    public static IRule<T, TResult> Then<T, TResult>(this IRule<T> previous, IRule<T, TResult> next)
        => new Rule<T, TResult>((s, ctx)
            => next.Apply(previous.Apply(s, ctx), ctx));

    public static IAsyncRule<T> Then<T>(this IAsyncRule<T> previous, IAsyncRule<T> next)
        => new AsyncRule<T>(async (s, ctx)
            => await next.Apply(await previous.Apply(s, ctx), ctx));

    public static IAsyncRule<T, TResult> Then<T, TResult>(this IAsyncRule<T> previous, IAsyncRule<T, TResult> next)
        => new AsyncRule<T, TResult>(async (s, ctx)
            => await next.Apply(await previous.Apply(s, ctx), ctx));

    public static IRule<T> Compose<T>(this IRule<T> next, IRule<T> previous)
        => new Rule<T>((s, ctx)
            => next.Apply(previous.Apply(s, ctx), ctx));

    public static IAsyncRule<T> Compose<T>(this IAsyncRule<T> next, IAsyncRule<T> previous)
        => new AsyncRule<T>(async (s, ctx)
            => await next.Apply(await previous.Apply(s, ctx), ctx));

    public static IRule<T> Join<T, TInner>(this IRule<T, TInner> first, IRule<TInner, T> second)
        => new Rule<T>((s, ctx)
            => second.Apply(first.Apply(s, ctx), ctx));

    public static IAsyncRule<T> Join<T, TInner>(this IAsyncRule<T, TInner> first, IAsyncRule<TInner, T> second)
        => new AsyncRule<T>(async (s, ctx)
            => await second.Apply(await first.Apply(s, ctx), ctx));

    extension<T>(IRule<T> rule)
    {
        public static IRule<T> operator |(IRule<T> left, IRule<T> right) => left.Then(right);
        public static IRule<T> operator &(IRule<T> left, IRule<T> right) => left.Compose(right);
    }

    extension<T, TResult>(IRule<T> rule)
    {
        public static IRule<T, TResult> operator |(IRule<T> left, IRule<T, TResult> right) => left.Then(right);
    }

    extension<T, TInner>(IRule<T, TInner> rule)
    {
        public static IRule<T> operator +(IRule<T, TInner> left, IRule<TInner, T> right) => left.Join(right);
    }

    extension<T>(IAsyncRule<T> rule)
    {
        public static IAsyncRule<T> operator |(IAsyncRule<T> left, IAsyncRule<T> right) => left.Then(right);
        public static IAsyncRule<T> operator &(IAsyncRule<T> left, IAsyncRule<T> right) => left.Compose(right);
    }

    extension<T, TResult>(IAsyncRule<T> rule)
    {
        public static IAsyncRule<T, TResult> operator |(IAsyncRule<T> left, IAsyncRule<T, TResult> right) => left.Then(right);
    }

    extension<T, TInner>(IAsyncRule<T, TInner> rule)
    {
        public static IAsyncRule<T> operator +(IAsyncRule<T, TInner> left, IAsyncRule<TInner, T> right) => left.Join(right);
    }
}
