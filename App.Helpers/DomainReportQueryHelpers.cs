using App.DTO.Common;
using DomainReport = App.Domain.DomainReport;

namespace App.Helpers;

public static class DomainReportQueryHelpers
{
    public static IQueryable<IGrouping<int, DomainReport>> GroupReports(this IQueryable<DomainReport> query, EDomainReportTimeframe timeframe)
    {
        return timeframe switch
        {
            EDomainReportTimeframe.Day => query.GetDayReports(),
            EDomainReportTimeframe.Week => query.GetWeekReports(),
            EDomainReportTimeframe.Month => query.GetMonthReports(),
            EDomainReportTimeframe.Year => query.GetYearReports(),
            _ => throw new ArgumentOutOfRangeException(nameof(timeframe), timeframe, "timeframe doesn't exist")
        };
    }
    
    private static IQueryable<IGrouping<int, DomainReport>> GetDayReports(this IQueryable<DomainReport> query)
    {
        var minimumDate = DateTime.UtcNow.AddDays(-1);
        return query
            .Where(x => x.CreatedAtUtc > minimumDate && x.CreatedAtUtc.Hour != minimumDate.Hour)
            .GroupBy(x => x.CreatedAtUtc.Hour);
    }

    private static IQueryable<IGrouping<int, DomainReport>> GetWeekReports(this IQueryable<DomainReport> query)
    {
        var minimumDate = DateTime.UtcNow.AddDays(-7);
        return query
            .Where(x => x.CreatedAtUtc > minimumDate && x.CreatedAtUtc.DayOfWeek != minimumDate.DayOfWeek)
            .GroupBy(x => x.CreatedAtUtc.Day);
    }

    private static IQueryable<IGrouping<int, DomainReport>> GetMonthReports(this IQueryable<DomainReport> query)
    {
        var minimumDate = DateTime.UtcNow.AddMonths(-1);
        return query
            .Where(x => x.CreatedAtUtc > minimumDate && x.CreatedAtUtc.Day != minimumDate.Day)
            .GroupBy(x => x.CreatedAtUtc.Day);
    }

    private static IQueryable<IGrouping<int, DomainReport>> GetYearReports(this IQueryable<DomainReport> query)
    {
        var minimumDate = DateTime.UtcNow.AddYears(-1);
        return query
            .Where(x => x.CreatedAtUtc > minimumDate && x.CreatedAtUtc.Month != minimumDate.Month)
            .GroupBy(x => x.CreatedAtUtc.Month);
    }
}