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
        var utcNow = DateTime.UtcNow;
        var minimumDate = utcNow.AddDays(-1).AddMinutes(60 - utcNow.Minute);
        minimumDate = new DateTime(minimumDate.Year, minimumDate.Month, minimumDate.Day, minimumDate.Hour, 0, 0, DateTimeKind.Utc);
        
        return query
            .Where(x => x.CreatedAtUtc > minimumDate)
            .GroupBy(x => x.CreatedAtUtc.Hour);
    }

    private static IQueryable<IGrouping<int, DomainReport>> GetWeekReports(this IQueryable<DomainReport> query)
    {
        var utcNow = DateTime.UtcNow;
        var minimumDate = utcNow.AddDays(-7).AddHours(24 - utcNow.Hour);
        minimumDate = new DateTime(minimumDate.Year, minimumDate.Month, minimumDate.Day, 0, 0, 0, DateTimeKind.Utc);
        
        return query
            .Where(x => x.CreatedAtUtc > minimumDate)
            .GroupBy(x => x.CreatedAtUtc.DayOfYear);
    }

    private static IQueryable<IGrouping<int, DomainReport>> GetMonthReports(this IQueryable<DomainReport> query)
    {
        var utcNow = DateTime.UtcNow;
        var minimumDate = utcNow.AddMonths(-1).AddHours(24 - utcNow.Hour);
        minimumDate = new DateTime(minimumDate.Year, minimumDate.Month, minimumDate.Day, 0, 0, 0, DateTimeKind.Utc);

        return query
            .Where(x => x.CreatedAtUtc > minimumDate)
            .GroupBy(x => x.CreatedAtUtc.DayOfYear);
    }

    private static IQueryable<IGrouping<int, DomainReport>> GetYearReports(this IQueryable<DomainReport> query)
    {
        var utcNow = DateTime.UtcNow;
        var minimumDate = utcNow.AddYears(-1).AddMonths(1);
        minimumDate = new DateTime(minimumDate.Year, minimumDate.Month, 1, 0, 0, 0, DateTimeKind.Utc);

        return query
            .Where(x => x.CreatedAtUtc > minimumDate)
            .GroupBy(x => x.CreatedAtUtc.Month);
    }
}