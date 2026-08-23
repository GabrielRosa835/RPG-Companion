namespace RpgCompanion.Core;

public interface IQuestion<TResponse>
{
    void Define(IQuestionBuilder<TResponse> builder, IQuestionContext context);
}
