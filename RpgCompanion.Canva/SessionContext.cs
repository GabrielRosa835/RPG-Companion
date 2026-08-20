namespace RpgCompanion.Host;

using Core;

public abstract record PlayerRole
{
    public record Unknown : PlayerRole;

    public record SessionMaster : PlayerRole;

    public record Default : PlayerRole;
}

public interface IPlayer
{
    public Guid Id { get; }
    public Guid SessionId { get; }
    public PlayerRole Role { get; }
}

public class SessionContext : ISessionContext
{
    public Guid Id { get; }
    public SessionPlayers Players { get; }
}

public class SessionPlayers
{
    private readonly IPlayer _master = default!;
    private readonly List<IPlayer> _defaults = [];
    private readonly List<IPlayer> _all = [];

    public IPlayer Master => _master;
    public IReadOnlyList<IPlayer> NonMasters => _defaults;
    public IReadOnlyList<IPlayer> All => _all;

    public SessionPlayers(IEnumerable<IPlayer> players)
    {
        foreach (var player in players)
        {
            if (player.Role is PlayerRole.Default)
            {
                _defaults.Add(player);
                _all.Add(player);
            }
            else if (player.Role is PlayerRole.SessionMaster)
            {
                if (_master is not null)
                {
                    throw new InvalidOperationException("A session can have at most a single master");
                }
                _master = player;
                _all.Add(player);
            }
        }
        if (_master is null)
        {
            throw new InvalidOperationException("A session should have at least single master");
        }
    }
}
