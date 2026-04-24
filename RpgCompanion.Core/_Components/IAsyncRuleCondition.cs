namespace RpgCompanion.Core;

using Engine.Contexts;

public interface IAsyncRuleCondition
{
    public Task<bool> ShouldApplyAsync(IContext e);
}

public delegate Task<bool> AsyncRuleCondition(IContext context);
