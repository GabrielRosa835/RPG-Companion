namespace RpgCompanion.Core;

public interface ISignalBuilder
{
    ISignalBuilder WithTargets(ISignalTargetPolicy targetPolicy);
}

public interface ISignalBuilder<TPayload> : ISignalBuilder
{
    ISignalBuilder WithPayload(TPayload payload);
}
