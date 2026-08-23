namespace RpgCompanion.Core;

public interface IRule<TSubject>
{
    RuleResult<TSubject> Apply(TSubject subject, IRuleContext context);
}

public interface IRule<TSubject, TResult>
{
    RuleResult<TResult> Apply(TSubject subject, IRuleContext context);
}
