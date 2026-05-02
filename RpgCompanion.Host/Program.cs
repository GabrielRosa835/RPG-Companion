using MediatR;
using RpgCompanion.Core;
using RpgCompanion.Host;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddSingleton<IComponentGraph, ComponentGraph>();
builder.Services.AddTransient<IRegistry, Registry>();
builder.Services.AddSingleton<ITrigger, Trigger>();

string pluginsFolder = builder.Configuration["PluginsFolder"]!;
var pluginsManager = new PluginManager();

builder.Services.AddSingleton(pluginsManager);
await pluginsManager.LoadAll(builder.Services, pluginsFolder);

builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssemblyContaining<Program>();
});

var host = builder.Build();

await host.StartAsync();

await pluginsManager.InitializeAll(host.Services);

await host.WaitForShutdownAsync();
