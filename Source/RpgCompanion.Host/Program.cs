using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace RpgCompanion.Application;

using Microsoft.Extensions.Logging;
using Utils.UnionTypes;

public static class Program
{
    public static void Main(string[] args)
    {
        var builder = Host.CreateApplicationBuilder(args);

        builder.Services.AddHost();
        builder.Logging.ClearProviders();

        var host = builder.Build();

        var manager = host.Services.GetRequiredService<PluginManager>();

        string pluginsDir = Path.Combine(Directory.GetCurrentDirectory(), "plugins");
        var found = manager.FindPlugins(pluginsDir);

        foreach (var p in found)
        {
            Console.WriteLine(p.Resource);
        }

        var plugin = manager.Collection["RpgCompanion.DnD"];

        var result = manager.Loader.Load(plugin).Result;
        if (result.TryGetFailure(out var failure))
        {
            failure.PrintDetails();
        }

        host.Run();
    }
}
