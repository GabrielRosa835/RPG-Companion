using MediatR;
using RpgCompanion.Core;
using RpgCompanion.Host.Plugins;
using RpgCompanion.Prototypes.MediatR;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddSingleton<IComponentGraph, ComponentGraph>();
builder.Services.AddTransient<IRegistry, Registry>();
builder.Services.AddSingleton<ITrigger, Trigger>();

string pluginsFolder = builder.Configuration["PluginsFolder"]!;
var pluginsManager = new PluginManager();

builder.Services.AddSingleton(pluginsManager);
await pluginsManager.LoadAll(builder.Services, pluginsFolder);

// 1. Add MediatR
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssemblyContaining<Program>();
});

// 2. Register your handler as an Open Generic.
// The DI container will automatically construct EventRaisedHandler<Attack.Event>,
// EventRaisedHandler<DiceRoll.Event>, etc., when requested by MediatR.
builder.Services.AddTransient(typeof(INotificationHandler<>), typeof(EventRaisedHandler<>));

var host = builder.Build();

await host.StartAsync();

await pluginsManager.InitializeAll(host.Services);

await host.WaitForShutdownAsync();
