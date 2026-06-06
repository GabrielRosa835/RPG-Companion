namespace RpgCompanion.Core;

public static class RuleExtensions
{
    extension<T>(IRule<T> rule)
    {
        public IRule<T> Then(IRule<T> next) => IRule<T>.Of(t => next.Apply(rule.Apply(t)));
        public IRule<T> Compose(IRule<T> before) => IRule<T>.Of(t => rule.Apply(before.Apply(t)));
        public static IRule<T> operator |(IRule<T> left, IRule<T> right) => left.Then(right);
        public static IRule<T> operator &(IRule<T> left, IRule<T> right) => left.Compose(right);
    }
}
