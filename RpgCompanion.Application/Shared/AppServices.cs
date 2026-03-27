namespace RpgCompanion.Application;

using Core.Engine;
using Engines;
using Microsoft.Extensions.DependencyInjection;
using Reflection;

public static class AppServices
{
    public static void AddAppServices(this IServiceCollection services)
    {
        services.AddSingleton<ITrigger, Trigger>();
        services.AddSingleton<PluginManager>();
        services.AddSingleton<EventQueue>();
        services.AddSingleton<Reflect>();

        services.AddHostedService<EngineJob>();
        services.AddScoped<Engine>();
    }

    public static void AddAppServices(this IServiceCollection pluginServices, IServiceProvider appServices)
    {
        pluginServices.AddSingleton(_ => appServices.GetRequiredService<PluginManager>());
        pluginServices.AddSingleton(_ => appServices.GetRequiredService<Reflect>());
        pluginServices.AddSingleton(_ => appServices.GetRequiredService<EventQueue>());
        pluginServices.AddSingleton(_ => appServices.GetRequiredService<ITrigger>());
    }
}
