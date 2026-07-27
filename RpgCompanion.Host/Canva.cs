namespace RpgCompanion.Host;

public static class Canva
{
    public static void Teste()
    {
        IServiceCollection mainServices = new ServiceCollection();

        mainServices.AddSingleton<IDK>();

        IServiceProvider mainProvider = mainServices.BuildServiceProvider();
        IServiceScopeFactory mainScopeFactory = mainProvider.GetRequiredService<IServiceScopeFactory>();

        IServiceCollection pluginServices = new ServiceCollection();

        pluginServices.AddSingleton<IDKPlugin>();
        pluginServices.AddSingleton<IServiceProvider>(_ => mainProvider);
        pluginServices.AddSingleton<IServiceScopeFactory>(_ => mainScopeFactory);

        IServiceProvider pluginProvider = pluginServices.BuildServiceProvider();

        IDKPlugin idk = pluginProvider.GetRequiredService<IDKPlugin>();
        IServiceProvider who = idk.Provider;
    }

    public record IDK;
    public record IDKPlugin(IServiceProvider Provider);
}
