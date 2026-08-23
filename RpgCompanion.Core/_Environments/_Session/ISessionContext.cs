namespace RpgCompanion.Core;

/// <summary>
/// Session being currently played
/// </summary>
public interface ISessionContext
{
    public IReadOnlyList<IPlayer> DefaultPlayers { get; }
    public IReadOnlyList<IPlayer> AllPlayers { get; }
    public IPlayer SessionMaster { get; }
}
