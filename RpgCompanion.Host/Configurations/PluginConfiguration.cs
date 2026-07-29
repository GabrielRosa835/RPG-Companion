namespace RpgCompanion.Host.Configuration;

using Toolbox;

internal class PluginConfiguration(
    IServiceCollection _services,
    EventArchives _eventArchives,
    IntentArchives _intentArchives,
    EntityArchives _entityArchives)
    : IPluginConfiguration
{
    internal PluginKey _key = new(Guid.CreateVersion7().ToString());
    internal Maybe<string?> _identifier;
    internal string? _name;
    internal string? _version;
    internal Action? _initializationRegistration;
    internal readonly Dictionary<Type, Action> _intentRegistrations = new();
    internal readonly Dictionary<Type, Action> _eventRegistrations = new();
    internal readonly Dictionary<Type, Action> _entityRegistrations = new();

    public PluginDescriptor Build()
    {
        var descriptor = new PluginDescriptor
        {
            Key = _key,
            Identifier = VerifyIdentifier(_identifier),
            Name = _name,
            Version = _version,
        };
        _initializationRegistration?.Invoke();
        foreach(var registration in _intentRegistrations.Values)
        {
            registration();
        }
        foreach(var registration in _eventRegistrations.Values)
        {
            registration();
        }
        foreach(var registration in _entityRegistrations.Values)
        {
            registration();
        }
        return descriptor;
    }

    public void WithKey(PluginKey key)
    {
        _key = key;
    }

    public void WithName(string name)
    {
        _name = name;
    }

    public void WithVersion(string version)
    {
        _version = version;
    }

    public void WithIdentifier(string identifier)
    {
        _identifier = identifier;
    }

    public void WithInitialization<TInitialization>() where TInitialization : class, IInitialization
    {
        _initializationRegistration = () => _services.AddTransient<IInitialization, TInitialization>();
    }

    public void WithAsyncInitialization<TInitialization>() where TInitialization : class, IAsyncInitialization
    {
        _initializationRegistration = () => _services.AddTransient<IAsyncInitialization, TInitialization>();
    }

    public void AddIntent<TIntent>(Action<IIntentConfiguration<TIntent>> configure) where TIntent : IIntent
    {
        _intentRegistrations[typeof(TIntent)] = () =>
        {
            var configuration = new IntentConfiguration<TIntent>(
                _key,
                _intentArchives,
                _services);
            configure(configuration);
            configuration.Commit();
        };
    }

    public void AddIntent<TIntent, TResult>(Action<IIntentConfiguration<TIntent, TResult>> configure) where TIntent : IIntent<TResult>
    {
        _intentRegistrations[typeof(TIntent)] = () =>
        {
            var configuration = new IntentResultConfiguration<TIntent, TResult>(
                _key,
                _intentArchives,
                _services);
            configure(configuration);
            configuration.Commit();
        };
    }

    public void AddEvent<TEvent>(Action<IEventConfiguration<TEvent>>? configure) where TEvent : Event
    {
        _eventRegistrations[typeof(TEvent)] = () =>
        {
            var configuration = new EventConfiguration<TEvent>(
                _key,
                _eventArchives,
                _services);
            configure?.Invoke(configuration);
            configuration.Commit();
        };
    }

    public void AddEntity<TEntity>(Action<IEntityConfiguration<TEntity>>? configure) where TEntity : IEntity
    {
        _entityRegistrations[typeof(TEntity)] = () =>
        {
            var configuration = new EntityConfiguration<TEntity>(
                _key,
                _entityArchives,
                _services);
            configure?.Invoke(configuration);
            configuration.Commit();
        };
    }

    private static string VerifyIdentifier(Maybe<string?> identifier)
    {
        if (!identifier.TryGetValue(out var identifierValue))
        {
            throw new InvalidOperationException("Identifier must be specified");
        }
        if (identifierValue is null)
        {
            throw new InvalidOperationException("Identifier cannot be null");
        }

        identifierValue = identifierValue.Trim();

        if (string.IsNullOrWhiteSpace(identifierValue))
        {
            throw new FormatException("Identifier cannot be empty");
        }
        if (identifierValue.Contains(" "))
        {
            throw new FormatException("Identifier cannot have spaces");
        }

        return identifierValue;
    }
}
