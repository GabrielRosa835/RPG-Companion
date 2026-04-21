namespace RpgCompanion.Host.Configuration;

using Core.Meta;
using Delegates;
using Descriptors;
using Microsoft.Extensions.DependencyInjection;

internal class InitializationConfiguration(IServiceCollection services, PluginKey _pluginKey)
{
    public InitializationConfiguration WithAction(InitializerAction action)
    {
        services.AddKeyedTransient(_pluginKey, (sp, key) => action);
        services.AddKeyedTransient<InitializationActionHandler>(_pluginKey, (sp, key) =>
        {
            var keyedDelegate = sp.GetRequiredKeyedService<InitializerAction>(key);
            return new InitializationActionHandler(keyedDelegate);
        });
        return this;
    }

    public InitializationConfiguration WithComponent<TInitializer>() where TInitializer : class, IInitializer
    {
        services.AddKeyedSingleton<IInitializer, TInitializer>(_pluginKey);
        return this;
    }
}
