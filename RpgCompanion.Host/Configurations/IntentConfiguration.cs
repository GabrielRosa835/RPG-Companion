namespace RpgCompanion.Host.Configuration;

internal class IntentConfiguration<TIntent>(
    PluginKey _pluginKey,
    IntentArchives _intentArchives,
    IServiceCollection _services)
    : IIntentConfiguration<TIntent> where TIntent : IIntent
{
    private IntentKey _key = new(Guid.CreateVersion7().ToString());
    private string? _name;
    private Action? _processorRegistration;
    private Type? _processorType;

    internal void Commit()
    {
        ArgumentNullException.ThrowIfNull(_processorType);
        var descriptor = new IntentDescriptor
        {
            PluginKey = _pluginKey,
            Key = _key,
            Name = _name,
            Type = typeof(TIntent),
            ProcessorType = _processorType,
        };
        _processorRegistration?.Invoke();
        _services.AddKeyedSingleton(_key, descriptor);
        _intentArchives.Add(descriptor);
    }

    public void WithKey(IntentKey key)
    {
        _key = key;
    }

    public void WithName(string name)
    {
        _name = name;
    }

    public void WithProcessor<TProcessor>() where TProcessor : class, IIntentProcessor<TIntent>
    {
        _processorType = typeof(TProcessor);
        _processorRegistration = () =>
        {
            _services.AddTransient<IIntentProcessor<TIntent>, TProcessor>();
            _services.AddKeyedSingleton<IntentExecutor>(_key, new IntentExecutor.Sync<TIntent>());
        };
    }

    public void WithAsyncProcessor<TProcessor>() where TProcessor : class, IAsyncIntentProcessor<TIntent>
    {
        _processorType = typeof(TProcessor);
        _processorRegistration = () =>
        {
            _services.AddTransient<IAsyncIntentProcessor<TIntent>, TProcessor>();
            _services.AddKeyedSingleton<IntentExecutor>(_key, new IntentExecutor.Async<TIntent>());
        };
    }
}
