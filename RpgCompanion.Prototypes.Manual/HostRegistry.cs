namespace RpgCompanion.Application;

using Engine;
using Engines;
using Microsoft.Extensions.DependencyInjection;
using Reflection;

public static class HostRegistry
{
    public static void AddHost(this IServiceCollection services)
    {
        services.AddHostedService<EngineJob>();

        services.AddSingleton<EventExecutionProcess>();
        services.AddSingleton<ScopedServiceScope>();
        services.AddSingleton<EventQueue>();
        services.AddSingleton<Reflect>();

        services.AddSingleton<PluginCollection>();
        services.AddSingleton<PluginManager>();
        services.AddTransient<PluginBuilder>();
        services.AddTransient<PluginLoader>();
    }
}
