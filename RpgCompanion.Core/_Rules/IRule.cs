namespace RpgCompanion.Core;

public interface IRule<TSubject>
{
    TSubject Apply(TSubject subject, IRuleContext context);
}

public interface IRule<TSubject, TResult>
{
    TResult Apply(TSubject subject, IRuleContext context);
}
