namespace RpgCompanion.Application;

using System.Text;

public static class ExceptionExtensions
{
    extension(Exception? exception)
    {
        public void PrintDetails() => Console.WriteLine(exception?.Details());

        private void BuildMessage(StringBuilder stringBuilder, int currentDepth)
        {
            if (exception is null) return;

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
                return;
            }
            exception.InnerException?.BuildMessage(stringBuilder, currentDepth + 1);
        }
    }

    extension(Exception exception)
    {
        public string Details()
        {
            return $"{exception.FullMessage()}\n{exception.StackTrace}";
        }

        public string FullMessage()
        {
            var stringBuilder = new StringBuilder();
            exception.BuildMessage(stringBuilder, 0);
            return stringBuilder.ToString().TrimEnd();
        }
    }
}
