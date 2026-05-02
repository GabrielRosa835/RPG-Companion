namespace RpgCompanion.Core;

public interface IRuleDefinition<T, U>
{
    Rule<T, U> Compose();
}

public interface IRuleDefinition<T>
{
    Rule<T> Compose();
}
