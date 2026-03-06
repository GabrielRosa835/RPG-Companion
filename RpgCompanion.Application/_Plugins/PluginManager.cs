using System.Runtime.Loader;

using Microsoft.Extensions.DependencyInjection;

using RpgCompanion.Core.Meta;

using Utils.UnionTypes;

namespace RpgCompanion.Application;

internal class PluginManager
{
   private List<PluginDescriptor> _descriptors = [];
   public IReadOnlyList<PluginDescriptor> Descriptors => _descriptors;

   public PluginDescriptor this[int index] => _descriptors[index];
   public PluginDescriptor this[string name] => _descriptors.First(d => d.Name == name);

   public bool TryGetDescriptor (string name, out PluginDescriptor descriptor)
   {
      descriptor = _descriptors.FirstOrDefault(d => d.Name == name)!;
      return descriptor is not null;
   }

   public IReadOnlyList<PluginDescriptor> FindPlugins (string pluginsFolder)
   {
      if (!Directory.Exists(pluginsFolder))
      {
         throw new DirectoryNotFoundException(pluginsFolder);
      }
      _descriptors = Directory.GetFiles(pluginsFolder, "*.dll", SearchOption.AllDirectories)
          .Select(dll => new PluginDescriptor
          {
             Name = Path.GetFileNameWithoutExtension(dll),
             Path = Path.GetFullPath(dll),
          })
          .ToList();
      return _descriptors.AsReadOnly();
   }

   public Attempt Load (PluginDescriptor descriptor)
   {
      bool TryActivate<T> (Type? type, out T instance)
      {
         if (type is null)
         {
            instance = default!;
            return false;
         }

         instance = (T?) Activator.CreateInstance(type)!;
         return instance is not null;
      }
      try
      {
         var context = new AssemblyLoadContext(descriptor.Name, isCollectible: true);
         var assembly = context.LoadFromAssemblyPath(descriptor.Path);

         var systemType = assembly.GetTypes()
             .FirstOrDefault(t => typeof(ISystem).IsAssignableFrom(t) && t is { IsInterface: false, IsAbstract: false });

         if(!TryActivate(systemType, out ISystem system))
         {
            return Results.Failure();
         }

         var manifestType = assembly.GetTypes().FirstOrDefault(t =>
         {
            if (t.IsInterface || t.IsAbstract) return false;
            return t.GetInterfaces().Any(i =>
                   i.IsGenericType &&
                   i.GetGenericTypeDefinition() == typeof(IManifest<>) &&
                   i.GetGenericArguments()[0] == systemType);
         });

         if (!TryActivate(manifestType, out IManifest<ISystem> manifest))
         {
            return Results.Failure();
         }

         var services = new ServiceCollection();
         var registryCollection = new RegistryCollection(services);

         services.AddTransient(manifest.Initializer);

         manifest.Setup(registryCollection);

         var serviceProvider = services.BuildServiceProvider();
         var registry = new Registry(serviceProvider);

         var initializer = serviceProvider.GetRequiredService(manifest.Initializer);

         var method = manifestType!.GetMethod(nameof(IInitializer<>.Initialize));
         method?.Invoke(manifest, [registry]);

         descriptor.Assembly = assembly;
         descriptor.System = system;
         descriptor.Registry = registry;
         descriptor.Activated = true;

         return Results.Success();
      }
      catch (Exception e)
      {
         return Results.Failure(e);
      }
   }
}