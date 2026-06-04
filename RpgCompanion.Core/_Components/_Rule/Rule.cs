namespace RpgCompanion.Core;

public static class Rule
{
    public static Rule<T, U> Of<T, U>(Func<T, U> rule) => new(rule);
    public static Rule<T> Raise<T>(T value) => new(_ => value);

    extension<T>(IRule<T> rule)
    {
        public IRule<T> Then(IRule<T> next) => new Rule<T>(t => next.Apply(rule.Apply(t)));
        public IRule<T> Compose(IRule<T> before) => new Rule<T>(t => rule.Apply(before.Apply(t)));

        public Rule<T> Cement() => new(rule.Apply);

        public static IRule<T> operator |(IRule<T> left, IRule<T> right) => left.Then(right);
        public static IRule<T> operator &(IRule<T> left, IRule<T> right) => left.Compose(right);
        public static Rule<T> operator ~(IRule<T> interfaced) => interfaced.Cement();
    }

    extension<T>(T value)
    {
        public IRule<T> Suspend() => Raise(value);
    }
}
