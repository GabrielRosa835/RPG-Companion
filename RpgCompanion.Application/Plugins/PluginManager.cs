using System.Runtime.Loader;

using Microsoft.Extensions.DependencyInjection;
using RpgCompanion.Application.Reflection;
using RpgCompanion.Core.Meta;

using Utils.UnionTypes;

namespace RpgCompanion.Application;

internal class PluginManager
{
   private readonly List<PluginDescriptor> _descriptors = [];
   public IReadOnlyList<PluginDescriptor> Descriptors => _descriptors.AsReadOnly();

   public PluginDescriptor this[int index] => _descriptors[index];
   public PluginDescriptor this[string name] => _descriptors.First(d => d.Resource == name);

   public IReadOnlyList<PluginDescriptor> FindPlugins (string pluginsFolder)
   {
      if (!Directory.Exists(pluginsFolder))
      {
         throw new DirectoryNotFoundException(pluginsFolder);
      }
      _descriptors.AddRange(Directory.GetFiles(pluginsFolder, "*.dll", SearchOption.AllDirectories)
          .Select(dll => new PluginDescriptor
          {
             Resource = Path.GetFileNameWithoutExtension(dll),
             Path = Path.GetFullPath(dll),
          })
          .Where(d => !_descriptors.Contains(d)));
      return Descriptors;
   }

   public Task<Attempt> Load (PluginDescriptor plugin)
   {
      if (plugin.Activated)
      {
         return Task.FromResult(Results.Success());
      }
      return LoadInternal(plugin);
   }
   public Task<Attempt> ForceLoad (PluginDescriptor plugin)
   {
      return LoadInternal(plugin);
   }

   internal static Task<Attempt> LoadInternal (PluginDescriptor plugin) => Task.Run(() =>
   {
      try
      {
         var context = new AssemblyLoadContext(plugin.Resource, isCollectible: true);
         var assembly = context.LoadFromAssemblyPath(plugin.Path);

         var manifestType = assembly.GetTypes().FirstOrDefault(
            t => t.Implements(typeof(IManifest)));
         
         if (manifestType is null || Activator.CreateInstance(manifestType) is not IManifest manifest)
         {
            return Results.Failure();
         }

         var pluginBuilder = new PluginBuilder();
         manifest.Setup(pluginBuilder);
         var registry = pluginBuilder.Build();

         if (pluginBuilder.InitializerType is not null)
         {
            var initializer = (IInitializer?) registry.Provider.GetService(pluginBuilder.InitializerType);
            initializer?.Initialize(registry);
         }
         pluginBuilder.Initialization?.Invoke(registry);

         plugin.Assembly = assembly;
         plugin.Identifier = new(pluginBuilder.PluginName, pluginBuilder.PluginVersion);
         plugin.Registry = registry;
         plugin.Activated = true;

         return Results.Success();
      }
      catch (Exception e)
      {
         return Results.Failure(e);
      }
   });
}