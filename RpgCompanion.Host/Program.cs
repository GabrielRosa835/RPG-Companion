using RpgCompanion.Events;
using RpgCompanion.Host;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddSingleton<IComponentGraph, ComponentGraph>();
builder.Services.AddSingleton<EventPublisher>();
builder.Services.AddSingleton<Trigger>();
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

// Inside the Host's Program.cs or startup logic:

// A generic endpoint to catch all intents
// host.MapPost("/api/plugin/{pluginKey}/intent/{intentName}", async (
//     string pluginKey,
//     string intentName,
//     HttpContext context,
//     IIntentDispatcher dispatcher) => // IIntentDispatcher is an internal host service
// {
//     // 1. Read the raw JSON body
//     var rawJson = await new StreamReader(context.Request.Body).ReadToEndAsync();
//
//     // 2. The Host's dispatcher looks up the registered Type for 'intentName'
//     //    under the specific 'pluginKey'
//
//     // 3. Deserializes the JSON into the specific TIntent
//
//     // 4. Resolves the registered IIntentHandler from the DI container
//
//     // 5. Invokes HandleAsync() and returns the Result object as JSON
//     var result = await dispatcher.DispatchAsync(pluginKey, intentName, rawJson);
//
//     return Results.Ok(result);
// });

await host.StartAsync();

var serviceScopeFactory = host.Services.GetRequiredService<IServiceScopeFactory>();
await pluginsManager.InitializeAll(serviceScopeFactory);

await host.WaitForShutdownAsync();
