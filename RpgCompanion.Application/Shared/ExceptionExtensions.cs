namespace RpgCompanion.Application;

public static class ExceptionExtensions
{
    public static void PrintDetails(this Exception e) => Console.WriteLine($"[{e.GetType().Name}]: {e.Message} \n{e.StackTrace}");
}
