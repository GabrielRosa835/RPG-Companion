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

    #region Delegate Factories

    public static IRule<TSubject> Create<TSubject>(Func<TSubject, IRuleContext, RuleResult<TSubject>> handler)
        => new Rule<TSubject>(handler);

    public static IRule<TSubject, TResult> Create<TSubject, TResult>(Func<TSubject, IRuleContext, RuleResult<TResult>> handler)
        => new Rule<TSubject, TResult>(handler);

    public static IAsyncRule<TSubject> Create<TSubject>(Func<TSubject, IRuleContext, Task<RuleResult<TSubject>>> handler)
        => new AsyncRule<TSubject>(handler);

    public static IAsyncRule<TSubject, TResult> Create<TSubject, TResult>(Func<TSubject, IRuleContext, Task<RuleResult<TResult>>> handler)
        => new AsyncRule<TSubject, TResult>(handler);

    #endregion

    #region Then's

    public static IRule<T> Then<T>(this IRule<T> previous, IRule<T> next)
        => new Rule<T>((s, ctx) => previous.Apply(s, ctx).FlatMap(v => next.Apply(v, ctx)));

    public static IRule<T, TResult> Then<T, TResult>(this IRule<T> previous, IRule<T, TResult> next)
        => new Rule<T, TResult>((s, ctx) => previous.Apply(s, ctx).FlatMap(v => next.Apply(v, ctx)));

    public static IAsyncRule<T> Then<T>(this IAsyncRule<T> previous, IAsyncRule<T> next)
        => new AsyncRule<T>(async (s, ctx) => await previous.Apply(s, ctx).FlatMapAsync(async v => await next.Apply(v, ctx)));

    public static IAsyncRule<T> Then<T>(this IAsyncRule<T> previous, IRule<T> next)
        => new AsyncRule<T>(async (s, ctx) => await previous.Apply(s, ctx).FlatMap(v => next.Apply(v, ctx)));

    public static IAsyncRule<T> Then<T>(this IRule<T> previous, IAsyncRule<T> next)
        => new AsyncRule<T>(async (s, ctx) => await previous.Apply(s, ctx).FlatMapAsync(v => next.Apply(v, ctx)));

    public static IAsyncRule<T, TResult> Then<T, TResult>(this IAsyncRule<T> previous, IAsyncRule<T, TResult> next)
        => new AsyncRule<T, TResult>(async (s, ctx) => await previous.Apply(s, ctx).FlatMapAsync(v => next.Apply(v, ctx)));

    public static IAsyncRule<T, TResult> Then<T, TResult>(this IAsyncRule<T> previous, IRule<T, TResult> next)
        => new AsyncRule<T, TResult>(async (s, ctx) => await previous.Apply(s, ctx).FlatMap(v => next.Apply(v, ctx)));

    public static IAsyncRule<T, TResult> Then<T, TResult>(this IRule<T> previous, IAsyncRule<T, TResult> next)
        => new AsyncRule<T, TResult>(async (s, ctx) => await previous.Apply(s, ctx).FlatMapAsync(v => next.Apply(v, ctx)));

    #endregion

    #region Compose's

    public static IRule<T> Compose<T>(this IRule<T> next, IRule<T> previous)
        => new Rule<T>((s1, ctx) => previous.Apply(s1, ctx).FlatMap(s2 => next.Apply(s2, ctx)));

    public static IAsyncRule<T> Compose<T>(this IAsyncRule<T> next, IAsyncRule<T> previous)
        => new AsyncRule<T>(async (s1, ctx) => await previous.Apply(s1, ctx).FlatMapAsync(s2 => next.Apply(s2, ctx)));

    public static IAsyncRule<T> Compose<T>(this IAsyncRule<T> next, IRule<T> previous)
        => new AsyncRule<T>(async (s1, ctx) => await previous.Apply(s1, ctx).FlatMapAsync(s2 => next.Apply(s2, ctx)));

    public static IAsyncRule<T> Compose<T>(this IRule<T> next, IAsyncRule<T> previous)
        => new AsyncRule<T>(async (s1, ctx) => await previous.Apply(s1, ctx).FlatMap(s2 => next.Apply(s2, ctx)));

    #endregion

    #region Join's

    public static IRule<T> Join<T, TInner>(this IRule<T, TInner> first, IRule<TInner, T> second)
        => new Rule<T>((s1, ctx) => first.Apply(s1, ctx).FlatMap(s2 => second.Apply(s2, ctx)));

    public static IAsyncRule<T> Join<T, TInner>(this IAsyncRule<T, TInner> first, IAsyncRule<TInner, T> second)
        => new AsyncRule<T>(async (s1, ctx) => await first.Apply(s1, ctx).FlatMapAsync(s2 => second.Apply(s2, ctx)));

    public static IAsyncRule<T> Join<T, TInner>(this IAsyncRule<T, TInner> first, IRule<TInner, T> second)
        => new AsyncRule<T>(async (s1, ctx) => await first.Apply(s1, ctx).FlatMap(s2 => second.Apply(s2, ctx)));

    public static IAsyncRule<T> Join<T, TInner>(this IRule<T, TInner> first, IAsyncRule<TInner, T> second)
        => new AsyncRule<T>(async (s1, ctx) => await first.Apply(s1, ctx).FlatMapAsync(s2 => second.Apply(s2, ctx)));

    #endregion

    #region Operator's

    extension<T>(IRule<T> rule)
    {
        public static IRule<T> operator |(IRule<T> left, IRule<T> right) => left.Then(right);
        public static IRule<T> operator &(IRule<T> left, IRule<T> right) => left.Compose(right);
        public static IAsyncRule<T> operator |(IRule<T> left, IAsyncRule<T> right) => left.Then(right);
        public static IAsyncRule<T> operator &(IRule<T> left, IAsyncRule<T> right) => left.Compose(right);
    }

    extension<T, TResult>(IRule<T> rule)
    {
        public static IRule<T, TResult> operator |(IRule<T> left, IRule<T, TResult> right) => left.Then(right);
        public static IAsyncRule<T, TResult> operator |(IRule<T> left, IAsyncRule<T, TResult> right) => left.Then(right);
    }

    extension<T, TInner>(IRule<T, TInner> rule)
    {
        public static IRule<T> operator +(IRule<T, TInner> left, IRule<TInner, T> right) => left.Join(right);
        public static IAsyncRule<T> operator +(IRule<T, TInner> left, IAsyncRule<TInner, T> right) => left.Join(right);
    }

    extension<T>(IAsyncRule<T> rule)
    {
        public static IAsyncRule<T> operator |(IAsyncRule<T> left, IAsyncRule<T> right) => left.Then(right);
        public static IAsyncRule<T> operator &(IAsyncRule<T> left, IAsyncRule<T> right) => left.Compose(right);
        public static IAsyncRule<T> operator |(IAsyncRule<T> left, IRule<T> right) => left.Then(right);
        public static IAsyncRule<T> operator &(IAsyncRule<T> left, IRule<T> right) => left.Compose(right);
    }

    extension<T, TResult>(IAsyncRule<T> rule)
    {
        public static IAsyncRule<T, TResult> operator |(IAsyncRule<T> left, IAsyncRule<T, TResult> right) => left.Then(right);
        public static IAsyncRule<T, TResult> operator |(IAsyncRule<T> left, IRule<T, TResult> right) => left.Then(right);
    }

    extension<T, TInner>(IAsyncRule<T, TInner> rule)
    {
        public static IAsyncRule<T> operator +(IAsyncRule<T, TInner> left, IAsyncRule<TInner, T> right) => left.Join(right);
        public static IAsyncRule<T> operator +(IAsyncRule<T, TInner> left, IRule<TInner, T> right) => left.Join(right);
    }

    #endregion
}
