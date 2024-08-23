using App.Contracts.DAL.IRepositories;
using App.Domain.Enums;
using App.DTO.Common;
using App.Helpers;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;
using DomainReport = App.DTO.Common.DomainReport;

namespace App.DAL.EF.Repositories;

public class DomainReportRepository: EfBaseRepository<Domain.DomainReport, AppDbContext>, IDomainReportRepository
{
    public DomainReportRepository(AppDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<IEnumerable<DomainReport>> GetReports(string domain, EDomainReportTimeframe timeFrame)
    {
        var minimumDateTime = DomainReportQueryHelpers.GetMinimumDateTimeForQuery(timeFrame);
        var result = await DbSet.Include(x => x.WebDomain)
            .Where(x => x.WebDomain!.Name == domain && x.CreatedAtUtc > minimumDateTime)
            .GroupReports(timeFrame)
            .Select(g => new DomainReport
            {
                DateTime = g.Select(x => x.CreatedAtUtc).First(),
                ConnectionIssues = g.Count(x => x.ReportType == EReportType.ConnectionIssue)
            })
            .ToListAsync();

        return result;
    }

    public async Task AddReport(Guid domainId, Guid userId, EReportType reportType = EReportType.ConnectionIssue)
    {
        var now = DateTime.UtcNow.AddHours(-1);
        var reportExists = await DbSet.AnyAsync(x => x.UserId == userId && x.ReportType == reportType && x.WebDomainId == domainId && x.CreatedAtUtc > now);
        if (reportExists)
        {
            throw new Exception("User has already reported this domain in the last hour.");
        }

        var report = new Domain.DomainReport
        {
            ReportType = reportType,
            WebDomainId = domainId,
            UserId = userId
        };
        await DbSet.AddAsync(report);
    }
}