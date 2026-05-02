using MassTransit;
using RpgCompanion.Core;
using RpgCompanion.Host.Plugins;
using RpgCompanion.Prototypes.MassTransit;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddSingleton<IComponentGraph, ComponentGraph>();
builder.Services.AddTransient<IRegistry, Registry>();
builder.Services.AddSingleton<ITrigger, Trigger>();

string pluginsFolder = builder.Configuration["PluginsFolder"]!;
var pluginsManager = new PluginManager();

builder.Services.AddSingleton(pluginsManager);
await pluginsManager.LoadAll(builder.Services, pluginsFolder);

// 2. Build a temporary ServiceProvider to resolve the ComponentGraph.
// This allows us to inspect the loaded events before configuring MassTransit.
await using (var tempProvider = builder.Services.BuildServiceProvider())
{
    var componentGraph = tempProvider.GetRequiredService<IComponentGraph>();
    builder.Services.AddMassTransit(massTransit =>
    {
        foreach (var eventDescriptor in componentGraph.Events)
        {
            var closedConsumerType = typeof(EventRaisedConsumer<>).MakeGenericType(eventDescriptor.Type);
            massTransit.AddConsumer(closedConsumerType);
        }
        massTransit.UsingInMemory((context, configuration) => configuration.ConfigureEndpoints(context));
    });
}


var host = builder.Build();

await host.StartAsync();

await pluginsManager.InitializeAll(host.Services);

await host.WaitForShutdownAsync();
