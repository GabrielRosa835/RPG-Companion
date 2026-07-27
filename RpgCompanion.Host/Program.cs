using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using RpgCompanion.Host;
using RpgCompanion.Host.Database;
using RpgCompanion.Host.Events;
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

builder.Services.AddSingleton<IMongoClient>(sp =>
{
    var config = sp.GetRequiredService<IConfiguration>();
    return new MongoClient(config["Persistence:ConnectionStrings:Local"]);
});

builder.Services.AddScoped<IMongoDatabase>(sp =>
{
    var config = sp.GetRequiredService<IConfiguration>();
    var client = sp.GetRequiredService<IMongoClient>();
    return client.GetDatabase(config["Persistence:DatabaseName"]);
});

// 1. Register your custom serializers for Id and Rel
BsonSerializer.RegisterSerializationProvider(new AppSerializationProvider());

var host = builder.Build();

await host.StartAsync();

var pluginManager = host.Services.GetRequiredService<PluginManager>();
var pluginLoader = host.Services.GetRequiredService<PluginLoader>();
var pluginInitializer = host.Services.GetRequiredService<PluginInitializer>();
var configuration = host.Services.GetRequiredService<IConfiguration>();

var plugins = await pluginManager.FindPlugins(configuration[ConfigKeys.PluginsFolder]!);
var loadResults = await pluginLoader.LoadMany(plugins);
var loaded = loadResults.Where(r => r is ILoadResult.Completed).Select(r => ((ILoadResult.Completed) r).Metadata);
var initializationResults = await pluginInitializer.InitializeMany(plugins);

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
