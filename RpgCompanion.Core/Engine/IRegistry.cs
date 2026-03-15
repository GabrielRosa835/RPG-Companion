namespace RpgCompanion.Core.Engine;

public interface IRegistry
{
    public T? GetComponent<T>() where T : class;
    public T GetRequiredComponent<T>() where T : class;
}
