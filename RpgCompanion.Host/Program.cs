using RpgCompanion.Core;
using RpgCompanion.Host;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddSingleton<IComponentGraph, ComponentGraph>();
builder.Services.AddSingleton<IEventPublisher, EventPublisher>();
builder.Services.AddSingleton<ITrigger, Trigger>();
builder.Services.AddScoped<RuleContext, RuleContextImpl>();

string pluginsFolder = builder.Configuration["PluginsFolder"]!;
var pluginsManager = new PluginManager();

builder.Services.AddSingleton(pluginsManager);
await pluginsManager.LoadAll(builder.Services, pluginsFolder);

builder.Services.AddMediatR(configuration =>
{
    configuration.RegisterServicesFromAssemblyContaining<Program>();
});

var host = builder.Build();

await host.StartAsync();

await pluginsManager.InitializeAll(host.Services);

await host.WaitForShutdownAsync();
