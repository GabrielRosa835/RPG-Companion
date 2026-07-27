namespace RpgCompanion.Canva;

using Core;

public class Manifest : IManifest
{
    public void Configure(IPluginConfiguration plugin)
    {
        Log.Debug("Configuring RpgCompanion.Canvas");
        plugin.WithName("Canva");
        plugin.WithVersion("1.0.0");
        plugin.AddIntent<Intent>(i =>
        {
            i.WithProcessor<Intent>();
        });
        plugin.WithAsyncInitialization<Initialization>();
        Log.Debug("Finished Configuring RpgCompanion.Canvas");
    }
}

public class Initialization : IAsyncInitialization
{
    public async Task Initialize(IInitializationContext context, CancellationToken cancellationToken)
    {
        Log.Debug("Initializing RpgCompanion.Canvas");
        var trigger = context.Registry.Get<IEventTrigger>();
        var task3 = trigger.Raise(new Event3());
        var task2 = trigger.Raise(new Event2());
        var task1 = trigger.Raise(new Event1());
        await Task.WhenAll(task1, task2, task3);
        Log.Debug("Event result 1: {0}", task1.Task.Result.GetType().Name);
        Log.Debug("Event result 2: {0}", task2.Task.Result.GetType().Name);
        Log.Debug("Event result 3: {0}", task3.Task.Result.GetType().Name);
        Log.Debug("Finished Initializing RpgCompanion.Canvas");
    }
}
