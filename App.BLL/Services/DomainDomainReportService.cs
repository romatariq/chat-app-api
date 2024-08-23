using App.Contracts.BLL.IServices;
using App.Contracts.DAL;
using App.Domain.Enums;
using App.DTO.Common;
using App.Helpers;

namespace App.BLL.Services;

public class DomainDomainReportService : IDomainReportService
{
    protected IAppUOW Uow;

    public DomainDomainReportService(IAppUOW uow)
    {
        Uow = uow;
    }

    public async Task<IEnumerable<DomainReport>> GetReports(string domain, EDomainReportTimeframe timeFrame)
    {
        var result = new List<DomainReport>();

        var earliestReportDateTime = DomainReportQueryHelpers.GetMinimumDateTimeForQuery(timeFrame);
        var utcNow = DateTime.UtcNow;
        while (earliestReportDateTime < utcNow)
        {
            result.Add(new DomainReport{ DateTime = earliestReportDateTime });

            earliestReportDateTime = timeFrame switch
            {
                EDomainReportTimeframe.Day => earliestReportDateTime.AddHours(1),
                EDomainReportTimeframe.Week or EDomainReportTimeframe.Month => earliestReportDateTime.AddDays(1),
                EDomainReportTimeframe.Year => earliestReportDateTime.AddMonths(1),
                _ => throw new ArgumentOutOfRangeException(nameof(timeFrame), timeFrame, null)
            };
        }

        var reports = await Uow.DomainReportRepository.GetReports(domain, timeFrame);
        foreach (var queriedReport in reports)
        {
            var relatedDomainReport = result.First(report =>
                timeFrame == EDomainReportTimeframe.Day && report.DateTime.Hour == queriedReport.DateTime.Hour ||
                (timeFrame is EDomainReportTimeframe.Week or EDomainReportTimeframe.Month) && report.DateTime.DayOfYear == queriedReport.DateTime.DayOfYear ||
                timeFrame == EDomainReportTimeframe.Year && report.DateTime.Month == queriedReport.DateTime.Month);
            
            relatedDomainReport.ConnectionIssues = queriedReport.ConnectionIssues;
        }

        return result;
    }

    public async Task AddReport(Guid domainId, Guid userId, EReportType reportType = EReportType.ConnectionIssue)
    {
        await Uow.DomainReportRepository.AddReport(domainId, userId, reportType);
    }
}