using MassTransit;
using RpgCompanion.Core;
using RpgCompanion.Host;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddSingleton<IComponentLookup, ComponentLookup>();
builder.Services.AddSingleton<IComponentGraph, ComponentGraph>();
builder.Services.AddTransient<IRegistry, Registry>();
builder.Services.AddSingleton<ITrigger, Trigger>();

string pluginsFolder = builder.Configuration["PluginsFolder"]!;
var pluginsManager = new PluginManager();

builder.Services.AddSingleton(pluginsManager);
await pluginsManager.LoadAll(builder.Services, pluginsFolder);

builder.Services.AddMassTransit(massTransit =>
{
    massTransit.AddConsumer<EventRaisedRouter>();
    massTransit.UsingInMemory((context, configuration) =>
    {
        configuration.ConfigureJsonSerializerOptions(options =>
        {
            // Add your custom System.Text.Json Converter
            // options.Converters.Add();
            return options;
        });
        configuration.ConfigureEndpoints(context);
    });
});

var host = builder.Build();

await host.StartAsync();

await pluginsManager.InitializeAll(host.Services);

await host.WaitForShutdownAsync();
