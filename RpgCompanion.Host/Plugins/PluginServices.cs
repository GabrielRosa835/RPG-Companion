namespace RpgCompanion.Host;

using Events;
using HostExclusive;
using Intents;

internal static class PluginServices
{
    internal static void AddPluginServices(this IServiceCollection services, IServiceProvider hostServices)
    {
        services.AddTransient<IEventTrigger>(_ => hostServices.GetRequiredService<EventEngine>());
        services.AddTransient<IEnvironmentAccessor>(_ => hostServices.GetRequiredService<EnvironmentAccessor>());
        services.AddTransient<IIntentDispatcher>(_ => hostServices.GetRequiredService<IntentDispatcher>());
        services.AddTransient<DefaultEventFactory>();

        services.AddTransient<HostContext>(_ => hostServices.GetRequiredService<HostContext>());
        services.AddTransient<HostRegistry>(_ => hostServices.GetRequiredService<HostRegistry>());

        services.AddTransient<IHostContext>(sp => sp.GetRequiredService<HostContext>());
        services.AddTransient<IHostRegistry>(sp => sp.GetRequiredService<HostRegistry>());

        services.AddTransient<IDatabase>(_ => hostServices.GetRequiredService<IDatabase>());
    }
}
