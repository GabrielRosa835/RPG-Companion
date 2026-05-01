namespace RpgCompanion.Core;

public interface IRuleDefinition<T, U>
{
    Rule<T, U> Compose(RuleKey<T, U> key);
}

public interface IRuleDefinition<T>
{
    Rule<T> Compose(RuleKey<T> key);
}
