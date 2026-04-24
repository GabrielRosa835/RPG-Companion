namespace RpgCompanion.Core;

public interface IRegistry
{
    public T? Get<T>() where T : class;
    public T GetRequired<T>() where T : class;
}
