namespace RpgCompanion.Core;

public interface ISignal
{
    void Define(ISignalBuilder builder, ISignalContext context);
}

public interface ISignal<TPayload>
{
    void Define(ISignalBuilder<TPayload> builder, ISignalContext context);
}
