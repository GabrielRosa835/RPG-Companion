using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using RpgCompanion.Application.Reflection;
using RpgCompanion.Core.Engine;

namespace RpgCompanion.Application;

public static class AppServices
{
    public static void AddAppServices(this IServiceCollection services)
    {
        services.AddSingleton<PluginManager>();
        services.AddSingleton<Reflect>();
    }
}