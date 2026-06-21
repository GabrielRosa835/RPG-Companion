namespace RpgCompanion.Host;

using Core;

internal class RuleContextImpl : RuleContext
{
    private readonly IServiceProvider _serviceProvider;

    public RuleContextImpl(
        PluginExecutionContext pluginExecutionContext,
        IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        World = _serviceProvider.GetRequiredKeyedService<World>(pluginExecutionContext.Key);
    }

    public override TActor? GetOrDefault<TActor>() where TActor : class =>
        _serviceProvider.GetService<TActor>();

    public override TActor Get<TActor>() where TActor : class =>
        _serviceProvider.GetRequiredService<TActor>();
}
