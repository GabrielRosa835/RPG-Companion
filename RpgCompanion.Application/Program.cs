using System.Reflection;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Utils.UnionTypes;

namespace RpgCompanion.Application;

public static class Program
{
   public static void Main (string[] args)
   {
      var builder = Host.CreateApplicationBuilder(args);

      builder.Services.AddSingleton<PluginManager>();

      var host = builder.Build();
      
      var pluginManager = host.Services.GetRequiredService<PluginManager>();
      
      string executingAssembly = Assembly.GetExecutingAssembly().Location;
      Console.WriteLine(executingAssembly);
      
      string pluginsFolder = Path.Join(Path.GetDirectoryName(executingAssembly), "plugins");
      Console.WriteLine(pluginsFolder);
      
      pluginManager.FindPlugins(pluginsFolder);
      Console.WriteLine(pluginManager[0].Name);
      
      Console.WriteLine(pluginManager[0].System);
      var attempt = pluginManager.Load(pluginManager[0]);

      if (attempt.TryGetFailure(out var failure))
      {
         Console.WriteLine(failure);
      }
      else
      {
         Console.WriteLine(pluginManager[0].System.Name);
      }
   }
}