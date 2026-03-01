using Microsoft.Extensions.Hosting;

using RpgCompanion.Core.Meta;
using RpgCompanion.Core.Utils;

using System.Text.Json;

namespace RpgCompanion.Application;

public static class Program
{
   const string PLUGINS_FOLDER = @"C:\Storage\Programação\C#\RpgCompanion\RpgCompanion.Application\plugins\";
   public static void Main (string[] args)
   {
      var builder = Host.CreateApplicationBuilder(args);

      var registryCollection = new RegistryCollection(builder.Services);
      var plugins = new PluginLoader().LoadPlugins(PLUGINS_FOLDER, registryCollection);
      foreach (var plugin in plugins)
      {
         Console.WriteLine(JsonSerializer.Serialize(plugin));
      }

      var host = builder.Build();
      var registry = new Registry(host.Services);

      var initializers = registry.GetServices(typeof(IInitializer));
      var method = typeof(IInitializer).GetMethod(nameof(IInitializer.Initialize));
      foreach (var initializer in initializers)
      {
         method?.Invoke(initializer, [registry]);
      }
   }
}