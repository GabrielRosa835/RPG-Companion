namespace RpgCompanion.Core;

public static class RuleExtensions
{
    extension<T>(Rule<T> rule)
    {
        public Rule<T> Then(Rule<T> next)
        {
            return t => next(rule(t));
        }
        public Rule<T, U> Then<U>(Rule<T, U> next)
        {
            return t => next(rule(t));
        }
        public static Rule<T> operator |(Rule<T> first, Rule<T> next)
        {
            return first.Then(next);
        }
    }

    extension<T, U>(Rule<T, U> rule)
    {
        public Rule<T, V> Then<V>(Rule<U, V> next)
        {
            return t => next(rule(t));
        }
        public Rule<T> Then(Rule<U, T> next)
        {
            return t => next(rule(t));
        }
    }
}
