namespace RpgCompanion.Application.Engines;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Utils.UnionTypes;

internal class EngineJob(PluginManager pluginManager, IServiceProvider appProvider) : BackgroundService
{
    private readonly EventQueue _queue = new();

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            while (!_queue.Any())
            {
                await Task.Delay(100, stoppingToken);
            }

            var e = _queue.Dequeue();
            var plugin = pluginManager[e.Descriptor.Type];
            if (await pluginManager.Load(plugin).IsFailure())
            {
                return;
            }

            using var scope = appProvider.CreateScope();
            var engine = scope.ServiceProvider.GetRequiredService<Engine>();

            await engine.Execute(_queue, plugin.Registry, stoppingToken);
        }
    }
}
