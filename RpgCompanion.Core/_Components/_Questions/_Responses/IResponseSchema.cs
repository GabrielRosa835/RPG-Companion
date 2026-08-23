namespace RpgCompanion.Core;

public interface IResponseSchema<TPayload>
{
    public TPayload Process(IResponseContext<TPayload> context);
}
