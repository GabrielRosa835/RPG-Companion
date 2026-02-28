namespace Utils.Extensions;

public static class DateTimeExtensions
{
    /// <summary>
    /// Calculates the next occurrence of a specific time of day (represented by <paramref name="instant"/>) 
    /// relative to the given <paramref name="reference"/> DateTime.
    /// If the specified time has already passed on the reference day, the method returns the same time on the next day.
    /// </summary>
    /// <param name="reference">The reference DateTime from which to calculate the next occurrence.</param>
    /// <param name="instant">The time of day to find the next occurrence of, as a TimeSpan.</param>
    /// <returns>
    /// A DateTime representing the next occurrence of the specified time of day after the reference DateTime.
    /// </returns>
    public static DateTime NextOccurrenceOf(this DateTime reference, TimeSpan instant)
    {
        DateTime dueTime = reference.Date + instant;
        return dueTime <= reference 
            ? dueTime.AddDays(1) // If that time has already passed, it must be for tomorrow
            : dueTime; 
    }

    public static int GetRemainingBusinessHoursUntil(this DateTime startDate, DateTime endDate)
    {
        return (int) Math.Ceiling(startDate.CalculateBusinessTimeUntil(endDate).TotalHours);
    }

    public static int GetRemainingBusinessDaysUntil(this DateTime beginDate, DateTime endDate)
    {
        return (int) Math.Ceiling(beginDate.CalculateBusinessTimeUntil(endDate).TotalDays);
    }

    public static DateTime AddBusinessHours(this DateTime date, double hours)
    {
        if (hours == 0) return date;

        bool isNegative = hours < 0;
        double remainingHours = Math.Abs(hours);
        DateTime current = date;

        while (remainingHours > 0)
        {
            // Pula fins de semana
            while (current.DayOfWeek == DayOfWeek.Saturday || current.DayOfWeek == DayOfWeek.Sunday)
            {
                current = isNegative
                    ? current.AddDays(-1).Date.AddHours(23).AddMinutes(59).AddSeconds(59)
                    : current.AddDays(1).Date;
            }

            // Quanto tempo útil ainda existe hoje
            double availableHours = isNegative
                ? Math.Round((current - current.Date).TotalHours, 3)
                : Math.Round(24 - (current - current.Date).TotalHours, 3);

            if (availableHours <= 0)
            {
                current = isNegative
                    ? current.Date.AddDays(-1).AddHours(23).AddMinutes(59).AddSeconds(59)
                    : current.Date.AddDays(1);
                continue;
            }

            double hoursToApply = Math.Min(availableHours, remainingHours);
            current = isNegative
                ? current.AddHours(-hoursToApply)
                : current.AddHours(hoursToApply);

            remainingHours -= hoursToApply;
        }

        return current;
    }

    public static TimeSpan CalculateBusinessTimeUntil(this DateTime beginDate, DateTime endDate)
    {
        bool isNegative = beginDate > endDate;
        if (isNegative)
        {
            (beginDate, endDate) = (endDate, beginDate);
        }

        TimeSpan businessTime = TimeSpan.Zero;
        DateTime currentDate = beginDate;

        while (currentDate < endDate)
        {
            if (currentDate.DayOfWeek != DayOfWeek.Saturday && currentDate.DayOfWeek != DayOfWeek.Sunday)
            {
                // Calcula o intervalo até o final do dia ou até a data final
                DateTime nextDay = currentDate.Date.AddDays(1);
                DateTime endOfTheDay = nextDay > endDate ? endDate : nextDay;

                // Soma as horas úteis (diferença entre data atual e o fim do dia considerado)
                businessTime += (endOfTheDay - currentDate);
            }

            // Avança para o próximo dia
            currentDate = currentDate.Date.AddDays(1);
        }

        if (isNegative)
        {
            businessTime *= -1;
        }

        return businessTime;
    }
}