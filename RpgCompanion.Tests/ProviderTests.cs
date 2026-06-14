namespace RpgCompanion.Tests.Serialization;

using Core;
using Integration;
using Microsoft.Extensions.DependencyInjection;

// Implement the interface to inject the fixture
public class ProviderTests : IClassFixture<ServicesContainerFixture>
{
    private readonly ISerializationProvider _provider;

    public ProviderTests(ServicesContainerFixture fixture)
    {
        // Resolve exactly what you need from the shared DI container
        _provider = fixture.ServiceProvider.GetRequiredService<ISerializationProvider>();
    }

    // ... tests here
}
