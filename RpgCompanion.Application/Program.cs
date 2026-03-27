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

        builder.Services.AddAppServices();
        builder.Logging.ClearProviders();

        var host = builder.Build();

        var manager = host.Services.GetRequiredService<PluginManager>();
        var found = manager.FindPlugins("/Storage/Projects/C#/RPG-Companion/RpgCompanion.Application/bin/Debug/net10.0/plugins");

        foreach (var p in found)
        {
            Console.WriteLine(p.Resource);
        }

        var plugin = manager["RpgCompanion.Canva"];

        var result = manager.Load(plugin).Result;
        if (result.TryGetFailure(out var failure))
        {
            failure.PrintDetails();
        }

        host.Run();
    }

    private static void While(string[] args)
    {
        // while (true)
        // {
        //     var value = "";
        //     while (string.IsNullOrWhiteSpace(value))
        //     {
        //         value = Console.ReadLine();
        //     }
        //
        //     if (value == "exit")
        //     {
        //         Console.WriteLine("Shutting down...");
        //         break;
        //     }
        // }
    }
}
