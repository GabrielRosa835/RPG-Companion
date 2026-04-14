namespace RpgCompanion.QuartzPrototype.Configuration;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using RpgCompanion.Core.Meta;

public class InitializationBuilder(IServiceCollection services) : IInitializationBuilder
{
    public IInitializationBuilder WithAction(InitializerAction action)
    {
        services.TryAddSingleton(action);
        return this;
    }
    public IInitializationBuilder WithComponent<TInitializer>() where TInitializer : class, IInitializer
    {
        services.TryAddSingleton<IInitializer, TInitializer>();
        return this;
    }
}
