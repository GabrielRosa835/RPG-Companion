namespace RpgCompanion.Events;

public class Player
{
    public string ClientId;
    public IRole Role;
    public List<IAction> Actions { get; } = [];
}


public interface IRole
{
    public List<IAction> AllowedActions { get; }
}

public interface ISessionMaster : IRole;
