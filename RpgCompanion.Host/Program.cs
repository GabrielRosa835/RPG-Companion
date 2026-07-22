using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using RpgCompanion.Host;
using RpgCompanion.Host.Database;
using RpgCompanion.Host.Events;
using RpgCompanion.Host.Intents;

var builder = Host.CreateApplicationBuilder(args);

var pluginsFolder = builder.Configuration["PluginsFolder"] ?? throw new InvalidOperationException("PluginsFolder is missing.");
var loader = new PluginLoader(builder.Services, pluginsFolder);
var plugins = await loader.LoadAll();
var manager = new PluginManager(plugins);


#region Services

builder.Services.AddSingleton(manager);

builder.Services.AddSingleton<IEventTrigger, EventEngine>();
builder.Services.AddSingleton<IEnvironmentAccessor, EnvironmentAccessor>();
builder.Services.AddSingleton<DefaultEventFactory>();
builder.Services.AddSingleton<IIntentDispatcher, IntentDispatcher>();

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

#endregion

// 1. Register your custom serializers for Id and Rel
BsonSerializer.RegisterSerializationProvider(new AppSerializationProvider());


var host = builder.Build();

await host.StartAsync();

var scopeFactory = host.Services.GetRequiredService<IServiceScopeFactory>();
var initializer = new PluginInitializer(scopeFactory, plugins);
await initializer.InitializeAll();

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
