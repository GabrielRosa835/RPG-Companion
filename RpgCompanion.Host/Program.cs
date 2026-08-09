using RpgCompanion.Host;
using RpgCompanion.Host.Events;
using RpgCompanion.Host.HostExclusive;
using RpgCompanion.Host.Intents;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddSingleton<PluginArchives>();
builder.Services.AddSingleton<EntityArchives>();
builder.Services.AddSingleton<EventArchives>();
builder.Services.AddSingleton<IntentArchives>();

builder.Services.AddSingleton<PluginManager>();
builder.Services.AddSingleton<PluginLoader>();
builder.Services.AddSingleton<PluginInitializer>();

builder.Services.AddSingleton<EventEngine>();
builder.Services.AddSingleton<EnvironmentAccessor>();
builder.Services.AddSingleton<IntentDispatcher>();
builder.Services.AddSingleton<DefaultEventFactory>();
builder.Services.AddSingleton<IEnvironmentAccessor>(sp => sp.GetRequiredService<EnvironmentAccessor>());

builder.Services.AddTransient<HostContext>();
builder.Services.AddTransient<HostRegistry>();

builder.Services.AddLogging(log =>
{
    log.AddConsole();
    log.SetMinimumLevel(LogLevel.Trace);
    log.AddDebug();
});

var host = builder.Build();

await host.StartAsync();

var pluginManager = host.Services.GetRequiredService<PluginManager>();
var pluginLoader = host.Services.GetRequiredService<PluginLoader>();
var pluginInitializer = host.Services.GetRequiredService<PluginInitializer>();
var configuration = host.Services.GetRequiredService<IConfiguration>();
var logger = host.Services.GetRequiredService<ILogger<Program>>();

var plugins = await pluginManager.FindPlugins(configuration[ConfigKeys.PluginsFolder]!);
var loadResults = await pluginLoader.LoadMany(plugins);

var loaded = new List<PluginMetadata>();
foreach (var loadResult in loadResults)
{
    if (loadResult is LoadResult.Faulted faulted)
    {
        logger.LogError(faulted.Exception, "An exception occurred");
    }
    else if (loadResult is LoadResult.Completed completed)
    {
        logger.LogInformation("Plugin {0} successfully loaded", completed.Metadata.Manifest.Id);
        loaded.Add(completed.Metadata);
    }
}

var initializationResults = await pluginInitializer.InitializeMany(loaded);
foreach (var initializationResult in initializationResults)
{
    if (initializationResult is InitializationResult.Faulted faulted)
    {
        logger.LogError(faulted.Exception, "An exception occurred");
    }
    else if (initializationResult is InitializationResult.Completed completed)
    {
        logger.LogInformation("Plugin {0} successfully initialized", completed.Metadata.Manifest.Id);
    }
}

await host.WaitForShutdownAsync();

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
