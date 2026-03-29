namespace RpgCompanion.Application;

using System.Runtime.Versioning;
using System.Security.Cryptography.X509Certificates;
using Core.Engine;
using Core.Engine.Contexts;
using Engine;
using Engines;
using Microsoft.Extensions.DependencyInjection;
using Reflection;
using RpgCompanion.Engine.Components;
using Services;

internal static class AppServices
{
    internal static void AddPluginServices(
        this IServiceCollection pluginServices,
        IServiceProvider hostServices,
        ComponentCollection components)
    {
        pluginServices.AddTransient<EventExecutionProcess>(_ => hostServices.GetRequiredService<EventExecutionProcess>());
        pluginServices.AddTransient<ScopedServiceScope>(_ => hostServices.GetRequiredService<ScopedServiceScope>());
        pluginServices.AddTransient<EventQueue>(_ => hostServices.GetRequiredService<EventQueue>());
        pluginServices.AddTransient<Reflect>(_ => hostServices.GetRequiredService<Reflect>());

        pluginServices.AddSingleton<ContextData>();
        pluginServices.AddSingleton(components);

        pluginServices.AddScoped<EventExecutor>();
        pluginServices.AddScoped<Context>();

        pluginServices.AddTransient<ComponentProvider>();
        pluginServices.AddTransient<Registry>();
        pluginServices.AddTransient<Pipeline>();

        pluginServices.AddTransient<IRegistry>(sp => sp.GetRequiredService<Registry>());
        pluginServices.AddTransient<IPipeline>(sp => sp.GetRequiredService<Pipeline>());
        pluginServices.AddTransient<IContext>(sp => sp.GetRequiredService<Context>());
    }
}
