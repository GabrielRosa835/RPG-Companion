namespace RpgCompanion.Host;

using Core;

public static class ServiceProviderExtensions
{
    public static IRegistry AsRegistry(this IServiceProvider provider)
    {
        return provider as IRegistry;
    }
}
