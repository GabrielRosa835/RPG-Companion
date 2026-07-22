namespace RpgCompanion.Canva;

public static class Log
{
    public static void Debug(string message, params object[] args)
    {
        Console.WriteLine($"[{DateTime.Now:HH:mm:ss.sss} DBG] {message}", args);
    }
}
