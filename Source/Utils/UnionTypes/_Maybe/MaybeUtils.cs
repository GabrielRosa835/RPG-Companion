namespace Utils.UnionTypes;

public static class MaybeUtils
{
    public static Maybe<T> Clear<T>(this Maybe<T> maybe) => Maybe<T>.None();
}