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

      host.Run();
   }
}
