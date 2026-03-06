using System.Collections;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using RpgCompanion.Core.Engine;
using RpgCompanion.Core.Events;

namespace RpgCompanion.Application;

internal class Engine
{
   private readonly Dictionary<string, PluginDescriptor> _plugins = default!;
   private readonly PluginManager _pluginManager = default!;
   private readonly EventQueue _queue = default!;
   private MethodInfo? _method; 

   public async Task Execute(PluginDescriptor plugin)
   {
      while(!_queue.Any())
      {
         await Task.Delay(100);
      }

      if (!plugin.Activated)
      {
         var loadAttempt = _pluginManager.Load(plugin);
         if (loadAttempt.IsFailure)
         {
            return;
         }
      }

      IEvent @event = _queue.Dequeue();
      Type type = @event.GetType();
      var context = new Context();
      
      var templateType = typeof(IContextTemplate<>).MakeGenericType(type);
      var template = plugin.Provider.GetService(templateType);
      
      _method = templateType.GetMethod(nameof(IContextTemplate<>.Bundle));
      _method?.Invoke(template, [context]);

      var contractType = typeof(IEventContract<>).MakeGenericType(type);
      var contract = plugin.Provider.GetService(contractType);

      var validator = new ContextValidator();
      if (contract is not null)
      {
         validator.ValidateInputs(context, contract, type);
      }

      var handlerType = typeof(IEventHandler<>).MakeGenericType(type);
      IEnumerable handlers = plugin.Provider.GetServices(handlerType);
      _method = handlerType.GetMethod(nameof(IEventHandler<>.Handle));
      foreach (var handler in handlers)
      {
         _method?.Invoke(handler, [@event, context]);
      }

      if (contract is not null)
      {
         validator.ValidateOutputs(context, contract, type);
      }
   }
}