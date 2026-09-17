namespace RpgCompanion.Application;

using System.Text;

public static class ExceptionExtensions
{
    extension(Exception? exception)
    {
        public void PrintDetails() => Console.WriteLine(exception?.Details);

        private StringBuilder BuildMessage(StringBuilder stringBuilder, int currentDepth)
        {
            if (exception is null) return stringBuilder;

            string tabs = new('\t', currentDepth);

            stringBuilder.Append(tabs)
                .Append(exception.GetType().Name)
                .Append(": ")
                .AppendLine(exception.Message);

            if (exception is AggregateException aggregateException)
            {
                foreach (var innerException in aggregateException.InnerExceptions)
                {
                    innerException.BuildMessage(stringBuilder, currentDepth + 1);
                }
                return stringBuilder;
            }
            exception.InnerException?.BuildMessage(stringBuilder, currentDepth + 1);
            return stringBuilder;
        }
    }

    extension(Exception exception)
    {
        public string Details => $"{exception.FullMessage}\n{exception.StackTrace}";
        public string FullMessage => exception.BuildMessage(new StringBuilder(), 0).ToString().TrimEnd();
    }
}
