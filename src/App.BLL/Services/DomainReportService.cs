using App.Contracts.BLL.IServices;
using App.Contracts.DAL;
using App.Domain.Enums;
using App.Domain.Exceptions;
using App.DTO.Common;
using App.Helpers;

namespace App.BLL.Services;

public class DomainReportService : IDomainReportService
{
    protected IAppUOW Uow;

    public DomainReportService(IAppUOW uow)
    {
        Uow = uow;
    }

    public async Task<(IEnumerable<DomainReport> reports, string domain)> GetReports(string encodedUrl, EDomainReportTimeframe timeFrame)
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

        var domain = UrlHelpers.ParseEncodedUrl(encodedUrl).domain;
        var reports = (await Uow.DomainReportRepository.GetReports(domain, timeFrame)).ToList();

        foreach (var queriedReport in reports)
        {
            var relatedDomainReport = result.First(report =>
                timeFrame == EDomainReportTimeframe.Day && report.DateTime.Hour == queriedReport.DateTime.Hour ||
                (timeFrame is EDomainReportTimeframe.Week or EDomainReportTimeframe.Month) && report.DateTime.DayOfYear == queriedReport.DateTime.DayOfYear ||
                timeFrame == EDomainReportTimeframe.Year && report.DateTime.Month == queriedReport.DateTime.Month);
            
            relatedDomainReport.ConnectionIssues = queriedReport.ConnectionIssues;
        }

        return (result, domain);
    }

    public async Task AddReport(Guid domainId, Guid userId, EReportType reportType = EReportType.ConnectionIssue)
    {
        var canReport = await Uow.DomainReportRepository.CanReport(domainId, userId, reportType);
        if (!canReport)
        {
            throw new CustomUserBadInputException("User already has an active report for this domain.");
        }

        await Uow.DomainReportRepository.AddReport(domainId, userId, reportType);
    }

    public async Task<bool> CanReport(Guid domainId, Guid userId, EReportType reportType = EReportType.ConnectionIssue)
    {
        return await Uow.DomainReportRepository.CanReport(domainId, userId, reportType);
    }
}