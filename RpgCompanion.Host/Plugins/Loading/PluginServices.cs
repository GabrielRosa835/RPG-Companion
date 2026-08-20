namespace RpgCompanion.Host;

internal static class PluginServices
{
    internal static void AddPluginServices(this IServiceCollection services,
        IServiceProvider hostServices,
        PluginMetadata metadata)
    {
        services.AddPublicServices();
        services.AddHostExclusiveServices(hostServices);
        // services.AddTransient<HostContext>(_ => hostServices.GetRequiredService<HostContext>());
        // services.AddTransient<HostRegistry>(_ => hostServices.GetRequiredService<HostRegistry>());
        // services.AddTransient<IHostContext>(sp => sp.GetRequiredService<HostContext>());
        // services.AddTransient<IHostRegistry>(sp => sp.GetRequiredService<HostRegistry>());
        // services.AddTransient<IDatabase>(_ => hostServices.GetRequiredService<IDatabase>());
        // services.AddLiteDbStorage(metadata);
    }

    private static void AddPublicServices(this IServiceCollection services)
    {
        services.AddTransient<IEventTrigger>(sp => sp.GetRequiredService<EventEngine>());
        services.AddTransient<IEnvironmentAccessor>(sp => sp.GetRequiredService<EnvironmentAccessor>());
        services.AddTransient<IIntentDispatcher>(sp => sp.GetRequiredService<IntentDispatcher>());
        services.AddTransient<IEventFactory, DefaultEventFactory>();
    }

    private static void AddHostExclusiveServices(this IServiceCollection services, IServiceProvider hostServices)
    {
        services.AddSingleton<ScopeProvider>();
        services.AddSingleton<EventExecutionContext>();
        services.AddSingleton<EventEngine>();
        services.AddFromHost<EnvironmentAccessor>(hostServices);
        services.AddFromHost<IntentDispatcher>(hostServices);
        services.AddFromHost<PluginArchives>(hostServices);
        services.AddFromHost<PluginAccessor>(hostServices);
    }

    private static void AddFromHost<TService>(this IServiceCollection services, IServiceProvider hostServices) where TService : class
    {
        services.AddTransient<TService>(_ => hostServices.GetRequiredService<TService>());
    }

    private static void AddLiteDbStorage(this IServiceCollection services, PluginMetadata metadata)
    {
        services.AddScoped<IStorage, LiteDbDynamicStorage>();
        services.AddOptions<LiteDbStorageOptions>()
            .Configure(o =>
            {
                o.InMemory = true;
                o.Shared = false;
                o.PluginFolder = metadata.FolderPath;
            })
            .PostConfigure(o =>
            {
                var dataPath = Path.Combine(o.PluginFolder, "data");
                if (!Directory.Exists(dataPath))
                {
                    Directory.CreateDirectory(dataPath);
                }
            });
    }
}
