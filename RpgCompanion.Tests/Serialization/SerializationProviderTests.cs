using Microsoft.Extensions.Options;
using RpgCompanion.Core;
using RpgCompanion.Host;

namespace RpgCompanion.Tests.Serialization;

using Microsoft.Extensions.DependencyInjection;
using Moq;

public class SerializationProviderTests
{
    private readonly ISerializationProvider _provider;

    public SerializationProviderTests()
    {
        var services = new ServiceCollection();

        // Register the external serializer for MagicItem
        services.AddTransient<ISerializer<MagicItem>, MagicItemSerializer>();

        // Register the Provider
        services.AddSingleton<ISerializationProvider, SystemTextJsonSerializationProvider>();

        // Provide the Options and ScopeFactory
        var options = Options.Create(new SerializationOptions { Indented = false });
        services.AddSingleton(options);

        var serviceProvider = services.BuildServiceProvider();
        _provider = serviceProvider.GetRequiredService<ISerializationProvider>();
    }

    [Fact]
    public void SerializeAndDeserialize_StandardPoco_UsesDefaultSerializerAndSucceeds()
    {
        // Arrange
        var spell = new SpellPoco
        {
            Name = "Fireball",
            Level = 3,
            RequiresConcentration = false
        };

        // Act
        var json = _provider.Serialize(spell);
        var deserializedSpell = _provider.Deserialize<SpellPoco>(json);

        // Assert
        Assert.NotNull(deserializedSpell);
        Assert.Equal(spell.Name, deserializedSpell.Name);
        Assert.Equal(spell.Level, deserializedSpell.Level);
        Assert.Equal(spell.RequiresConcentration, deserializedSpell.RequiresConcentration);

        // Verify standard JSON camel casing applied by DefaultSerializer
        Assert.Contains("\"name\":\"Fireball\"", json);
        Assert.Contains("\"requiresConcentration\":false", json);
    }

    [Fact]
    public void SerializeAndDeserialize_ISerializableModel_InvokesModelMethods()
    {
        // Arrange
        var stats = new CharacterStats
        {
            Strength = 18,
            Dexterity = 14
        };

        // Act
        var json = _provider.Serialize(stats);
        var deserializedStats = _provider.Deserialize<CharacterStats>(json);

        // Assert
        Assert.NotNull(deserializedStats);
        Assert.Equal(stats.Strength, deserializedStats.Strength);
        Assert.Equal(stats.Dexterity, deserializedStats.Dexterity);

        // Verify the custom keys from CharacterStats.Serialize were used
        Assert.Contains("\"str\":18", json);
        Assert.Contains("\"dex\":14", json);
    }

    [Fact]
    public void SerializeAndDeserialize_ModelWithExternalSerializer_ResolvesFromDi()
    {
        // Arrange
        var item = new MagicItem
        {
            ItemName = "Vorpal Sword",
            RarityTier = 5
        };

        // Act
        var json = _provider.Serialize(item);
        var deserializedItem = _provider.Deserialize<MagicItem>(json);

        // Assert
        Assert.NotNull(deserializedItem);
        Assert.Equal(item.ItemName, deserializedItem.ItemName);
        Assert.Equal(item.RarityTier, deserializedItem.RarityTier);

        // Verify the snake_case keys from MagicItemSerializer were used
        Assert.Contains("\"item_name\":\"Vorpal Sword\"", json);
        Assert.Contains("\"rarity_tier\":5", json);
    }

    [Fact]
    public void Serialize_NullModel_OutputsJsonNull()
    {
        // Arrange
        SpellPoco? nullSpell = null;

        // Act
        var json = _provider.Serialize(nullSpell);

        // Assert
        Assert.Equal("null", json);
    }

    [Fact]
    public void Deserialize_NestedPoco_HandlesRecursionProperly()
    {
        // Arrange
        var wrapper = new NestedWrapper
        {
            Id = 1,
            Spell = new SpellPoco { Name = "Mage Armor", Level = 1, RequiresConcentration = false }
        };

        // Act
        var json = _provider.Serialize(wrapper);
        var deserializedWrapper = _provider.Deserialize<NestedWrapper>(json);

        // Assert
        Assert.NotNull(deserializedWrapper);
        Assert.Equal(wrapper.Id, deserializedWrapper.Id);
        Assert.NotNull(deserializedWrapper.Spell);
        Assert.Equal(wrapper.Spell.Name, deserializedWrapper.Spell.Name);
    }

    [Fact]
    public void Serialize_ValidCharacter_WritesCorrectFieldsToContext()
    {
        // Arrange
        var character = new PlayerCharacter { Name = "Aragorn", Level = 5 };

        // We use Moq to create a fake context to observe how the model interacts with it
        var contextMock = new Mock<ISerializationContext>();

        // Setup fluent chaining returns
        contextMock.Setup(c => c.Field(It.IsAny<string>())).Returns(contextMock.Object);
        contextMock.Setup(c => c.String(It.IsAny<string>())).Returns(contextMock.Object);
        contextMock.Setup(c => c.Number(It.IsAny<int>())).Returns(contextMock.Object);

        // Setup the Object scope delegate invocation
        contextMock.Setup(c => c.Object(It.IsAny<Action<ISerializationContext>>()))
                   .Callback<Action<ISerializationContext>>(action => action(contextMock.Object))
                   .Returns(contextMock.Object);

        // Act
        character.Serialize(contextMock.Object);

        // Assert - Verify the pipeline received the correct instructions
        contextMock.Verify(c => c.Field("Name"), Times.Once);
        contextMock.Verify(c => c.String("Aragorn"), Times.Once);

        contextMock.Verify(c => c.Field("Level"), Times.Once);
        contextMock.Verify(c => c.Number(5), Times.Once);
    }

    [Theory]
    [InlineData("Gandalf", 20)]
    [InlineData("Frodo", 1)]
    public void Deserialize_ValidData_ReturnsHydratedModel(string expectedName, int expectedLevel)
    {
        // Arrange
        var contextMock = new Mock<IDeserializationContext>();

        // Configure the mock to return specific values when asked
        contextMock.Setup(c => c.GetString("Name")).Returns(expectedName);
        contextMock.Setup(c => c.GetNumber<int>("Level")).Returns(expectedLevel);

        // Act
        var result = PlayerCharacter.Deserialize(contextMock.Object);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedName, result.Name);
        Assert.Equal(expectedLevel, result.Level);
    }

    // Helper class for the nested test
    private class NestedWrapper
    {
        public int Id { get; set; }
        public SpellPoco? Spell { get; set; }
    }
}
