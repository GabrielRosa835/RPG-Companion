namespace RpgCompanion.Events;

using Core;

public class Player
{
    public string ClientId;
    public IRole Role;
}


public interface IRole
{
    public List<IIntentBase> AllowedActions { get; }
}

public interface ISessionMaster : IRole;
