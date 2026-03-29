namespace RpgCompanion.Engine.Components;

using Core.Engine;
using Microsoft.Extensions.DependencyInjection;

public class Registry (IServiceProvider provider) : IRegistry
{
    public T? Get<T>() where T : class => provider.GetService<T>();
    public T GetRequired<T>() where T : class => provider.GetRequiredService<T>();
}
