namespace RpgCompanion.Core;

public interface IQuestionBuilder<TResponse>
{
    /// <summary>O modelo da resposta esperada</summary>
    public IQuestionBuilder<TResponse> WithSchema(IResponseSchema<TResponse> schema);

    /// <summary>Quem vai responder a pergunta</summary>
    public IQuestionBuilder<TResponse> WithTargets(IQuestionTargetPolicy questionTargets);

    /// <summary>Quem pode saber quem está respondendo a pergunta</summary>
    public IQuestionBuilder<TResponse> WithSecrecy(IQuestionSecrecyPolicy targets);

    /// <summary>Quem vai esperar a pergunta ser respondida</summary>
    public IQuestionBuilder<TResponse> WithBlocking(IQuestionBlockingPolicy targets);

    /// <summary>O texto da pergunta</summary>
    public IQuestionBuilder<TResponse> WithPrompt(string prompt);
}
