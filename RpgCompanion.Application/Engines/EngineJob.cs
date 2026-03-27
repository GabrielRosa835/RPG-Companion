namespace RpgCompanion.Application.Engines;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Utils.UnionTypes;

internal class EngineJob(PluginManager pluginManager, IServiceScopeFactory scopeFactory, EventQueue queue) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                while (queue.Count == 0)
                {
                    await Task.Delay(100, stoppingToken);
                }

                var e = queue.Dequeue();
                var plugin = pluginManager[e.Descriptor.Type];
                if (await pluginManager.Load(plugin).IsFailure())
                {
                    return;
                }

                using var scope = scopeFactory.CreateScope();
                var engine = scope.ServiceProvider.GetRequiredService<Engine>();

                await engine.Execute(plugin.Registry, e, stoppingToken);
            }
            catch (Exception e)
            {
                e.PrintDetails();
            }
        }
    }
}
