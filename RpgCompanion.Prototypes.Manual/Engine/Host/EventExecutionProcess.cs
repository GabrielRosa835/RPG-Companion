namespace RpgCompanion.Application.Engines;

using Engine;
using Microsoft.Extensions.DependencyInjection;
using Utils.UnionTypes;

// Singleton
internal class EventExecutionProcess(
    PluginManager pluginManager, // Singleton
    EventQueue queue, // Singleton
    ScopedServiceScope scope) // Singleton
{
    internal async Task Run(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                if (queue.Count == 0)
                {
                    await Task.Delay(100, stoppingToken);
                    scope.Reset();
                    continue;
                }

                var e = queue.Dequeue();

                var pluginAttempt = await pluginManager.TryLoadByEvent(e.Descriptor.Type);
                if (!pluginAttempt.TryGetSuccess(out var plugin))
                {
                    continue;
                }

                // TODO: Alinhar ScopedServiceScope para utilizar do escopo do plugin
                var pluginScope = plugin.Services.CreateScope();
                var executor = pluginScope.ServiceProvider.GetRequiredService<EventExecutor>();
                await executor.Execute(e, stoppingToken);
            }
            catch (Exception e)
            {
                e.PrintDetails();
            }
        }
    }
}
