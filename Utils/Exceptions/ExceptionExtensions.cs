namespace RpgCompanion.Application;

using System.Text;

public static class ExceptionExtensions
{
    public static void PrintDetails(this Exception? error) => Console.WriteLine(error?.Details());

    public static string Details(this Exception exception)
    {
        return $"{exception.FullMessage()}\n{exception.StackTrace}";
    }

    public static string FullMessage(this Exception exception)
    {
        var stringBuilder = new StringBuilder();
        exception.BuildMessage(stringBuilder, 0);
        return stringBuilder.ToString().TrimEnd();
    }

    private static void BuildMessage(this Exception? error, StringBuilder stringBuilder, int currentDepth)
    {
        if (error is null) return;

        string tabs = new('\t', currentDepth);

        stringBuilder.Append(tabs)
            .Append(error.GetType().Name)
            .Append(": ")
            .AppendLine(error.Message);

        if (error is AggregateException aggregateException)
        {
            foreach (var innerException in aggregateException.InnerExceptions)
            {
                innerException.BuildMessage(stringBuilder, currentDepth + 1);
            }
            return;
        }
        error.InnerException?.BuildMessage(stringBuilder, currentDepth + 1);
    }
}
