using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace RpgCompanion.Application;

public static class Program
{
   public static void Main (string[] args)
   {
      var builder = Host.CreateApplicationBuilder(args);

      builder.Services.AddAppServices();

      var host = builder.Build();
      
      var pluginManager = host.Services.GetRequiredService<PluginManager>();
      
      string executingAssembly = Assembly.GetExecutingAssembly().Location;
      Console.WriteLine(executingAssembly);
      
      string pluginsFolder = Path.Join(Path.GetDirectoryName(executingAssembly), "plugins");
      Console.WriteLine(pluginsFolder);
   }
}