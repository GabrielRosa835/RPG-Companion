namespace RpgCompanion.Toolbox;

public static class TimeSpanExtensions
{
    public static TimeSpan Sum(this IEnumerable<TimeSpan> timeSpans)
    {
        return timeSpans.Aggregate(TimeSpan.Zero, (current, timeSpan) => current + timeSpan);
    }
    public static TimeSpan Sum<TSource>(this IEnumerable<TSource> sources, Func<TSource, TimeSpan> selector)
    {
        return sources.Select(selector).Sum();
    }
}
