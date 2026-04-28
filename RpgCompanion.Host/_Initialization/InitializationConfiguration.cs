namespace RpgCompanion.Host;

using Core;

internal class InitializationConfiguration(
    IServiceCollection _services,
    PluginKey _plugin)
    : IInitializationConfiguration
{
    private readonly InitializationKey _key = new();

    public InitializationDescriptor Build()
    {
        var descriptor = new InitializationDescriptor
        {
            Key = _key,
            Plugin = _plugin,
        };
        _services.AddKeyedSingleton(_key, descriptor);
        return descriptor;
    }

    public IInitializationConfiguration Export(Initialization instance)
    {
        _services.AddKeyedSingleton(_key, instance);
        return this;
    }

    public IInitializationConfiguration Export(Func<IRegistry, InitializationKey, Initialization> factory)
    {
        _services.AddKeyedTransient(_key, (sp, key) => factory(sp.AsRegistry(), (InitializationKey) key));
        return this;
    }
}
