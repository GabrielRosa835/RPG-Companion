namespace Utils.Extensions;

public static class TimeSpanExtensions
{
    public static TimeSpan Sum(this IEnumerable<TimeSpan> timeSpans)
    {
        TimeSpan sum = TimeSpan.Zero;
        foreach (TimeSpan timeSpan in timeSpans)
        {
            sum += timeSpan;
        }
        return sum;
    }
    public static TimeSpan Sum<TSource>(this IEnumerable<TSource> sources, Func<TSource, TimeSpan> selector)
    {
        TimeSpan sum = TimeSpan.Zero;
        foreach (TimeSpan timeSpan in sources.Select(selector))
        {
            sum += timeSpan;
        }
        return sum;
    }
}