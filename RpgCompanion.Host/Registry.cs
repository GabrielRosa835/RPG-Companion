namespace RpgCompanion.Host;

using Core;

public class Registry(IServiceProvider _serviceProvider) : IRegistry
{
    public TService? Find<TService>() where TService : class => _serviceProvider.GetService<TService>();

    public TService Get<TService>() where TService : class => _serviceProvider.GetRequiredService<TService>();
}
