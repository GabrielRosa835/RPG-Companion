using RpgCompanion.Core.Engine;
using RpgCompanion.Core.Meta;

using System.Runtime.Loader;

namespace RpgCompanion.Application;

public class PluginLoader
{
   public List<ISystem> LoadPlugins (string pluginsPath, IRegistryCollection registryCollection)
   {
      var loadedSystems = new List<ISystem>();

      if (!Directory.Exists(pluginsPath)) return loadedSystems;

      var dlls = Directory.GetFiles(pluginsPath, "*.dll", SearchOption.AllDirectories);

      foreach (var dll in dlls)
      {
         if (dll.Contains("RpgCompanion"))
         {
            Console.WriteLine(dll);
         }

         var context = new AssemblyLoadContext(Path.GetFileName(dll), isCollectible: true);
         var assembly = context.LoadFromAssemblyPath(Path.GetFullPath(dll));

         // 1. Find the System Metadata Class
         var systemType = assembly.GetTypes()
             .FirstOrDefault(t => typeof(ISystem).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract);

         if (systemType is null)
         {
            continue;
         }

         if (Activator.CreateInstance(systemType) is ISystem systemInstance)
         {
            // 2. Find the Manifest associated with THIS system type
            // We look for a type that implements IManifest<systemType>
            var manifestType = assembly.GetTypes()
                .FirstOrDefault(t => IsManifestFor(t, systemType));

            if (manifestType is not null)
            {
               // 3. Instantiate Manifest and register services
               var manifestInstance = Activator.CreateInstance(manifestType);

               // Use reflection to call RegisterComponents since we don't know the T at compile time
               var method = manifestType.GetMethod(nameof(IManifest<ISystem>.RegisterInitializer));
               method?.Invoke(manifestInstance, [registryCollection]);

               method = manifestType.GetMethod(nameof(IManifest<ISystem>.RegisterComponents));
               method?.Invoke(manifestInstance, [registryCollection]);
            }

            loadedSystems.Add(systemInstance);
         }
      }
      return loadedSystems;
   }

   private bool IsManifestFor (Type type, Type systemType)
   {
      if (type.IsInterface || type.IsAbstract) return false;

      return type.GetInterfaces().Any(i =>
          i.IsGenericType &&
          i.GetGenericTypeDefinition() == typeof(IManifest<>) &&
          i.GetGenericArguments()[0] == systemType);
   }
}