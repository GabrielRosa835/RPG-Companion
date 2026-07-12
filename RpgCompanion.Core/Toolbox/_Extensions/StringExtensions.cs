namespace RpgCompanion.Toolbox;

public static class StringExtensions
{
    extension(string value)
    {
        public string Format(params object[] args)
        {
            return string.Format(value, args);
        }
    }
}
