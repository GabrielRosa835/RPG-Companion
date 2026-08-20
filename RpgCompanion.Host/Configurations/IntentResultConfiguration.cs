namespace RpgCompanion.Host;

internal class IntentResultConfiguration<TIntent, TResult>(
    PluginKey _pluginKey,
    IntentArchives _intentArchives,
    IServiceCollection _services)
    : IIntentConfiguration<TIntent, TResult> where TIntent : IIntent<TResult>
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

    public void WithKey(string key)
    {
        _key = new IntentKey(key);
    }

    public void WithName(string name)
    {
        _name = name;
    }

    public void WithProcessor<TProcessor>() where TProcessor : class, IIntentProcessor<TIntent, TResult>
    {
        _processorType = typeof(TProcessor);
        _processorRegistration = () =>
        {
            _services.AddTransient<IIntentProcessor<TIntent, TResult>, TProcessor>();
            _services.AddKeyedSingleton<IntentExecutor>(_key, new IntentExecutor.SyncResult<TIntent, TResult>());
        };
    }

    public void WithAsyncProcessor<TProcessor>() where TProcessor : class, IAsyncIntentProcessor<TIntent, TResult>
    {
        _processorType = typeof(TProcessor);
        _processorRegistration = () =>
        {
            _services.AddTransient<IAsyncIntentProcessor<TIntent, TResult>, TProcessor>();
            _services.AddKeyedSingleton<IntentExecutor>(_key, new IntentExecutor.AsyncResult<TIntent, TResult>());
        };
    }
}
