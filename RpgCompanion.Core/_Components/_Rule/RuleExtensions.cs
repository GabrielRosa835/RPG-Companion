namespace RpgCompanion.Core;

public static class RuleExtensions
{
    extension<T>(Rule<T> rule)
    {
        public Rule<T> Then(Rule<T> next) => (t, c) => next.Invoke(rule.Invoke(t, c), c);
        public Rule<T> Compose(Rule<T> first) => (t, c) => rule.Invoke(first.Invoke(t, c), c);
        public static Rule<T> operator |(Rule<T> left, Rule<T> right) => left.Then(right);
        public static Rule<T> operator &(Rule<T> left, Rule<T> right) => left.Compose(right);
    }
}
