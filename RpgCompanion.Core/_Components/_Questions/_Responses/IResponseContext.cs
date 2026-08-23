namespace RpgCompanion.Core;

public interface IResponseContext<TResponsePayload>
{
    public IResponseSchema<TResponsePayload> Schema { get; }
}
