namespace RpgCompanion.Tests.Integration;

using Microsoft.Extensions.DependencyInjection;

public class ServicesContainerFixture : IDisposable
{
    public IServiceProvider ServiceProvider { get; }

    public ServicesContainerFixture()
    {
        var services = new ServiceCollection();
        // Add all your core services, MediatR handlers, providers, etc.
        ServiceProvider = services.BuildServiceProvider();
    }

    public void Dispose()
    {
        // Cleanup if necessary
    }
}
