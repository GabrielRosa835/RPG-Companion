namespace RpgCompanion.Core;

public interface IQuestionPublisher
{
    public Task<ResponseResult> Ask<TResponse>(IQuestion<TResponse> question, CancellationToken cancellationToken = default);
}
