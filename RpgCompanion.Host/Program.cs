using System.Text.Json.Serialization;
using MassTransit;
using RpgCompanion.Core;
using RpgCompanion.Host;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddSingleton<IComponentLookup, ComponentLookup>();
builder.Services.AddSingleton<IComponentGraph, ComponentGraph>();
builder.Services.AddTransient<IRegistry, Registry>();
builder.Services.AddSingleton<ITrigger, Trigger>();
builder.Services.AddOptions<SerializationOptions>()
   .Bind(builder.Configuration.GetSection(SerializationOptions.SectionName));

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
         options.Converters.Add(new JsonStringEnumConverter());
         return options;
      });
      configuration.ConfigureEndpoints(context);
   });
});

var host = builder.Build();

await host.StartAsync();

await pluginsManager.InitializeAll(host.Services);

await host.WaitForShutdownAsync();
