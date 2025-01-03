using App.Domain.Enums;
using App.DTO.Common;

namespace App.Contracts.DAL.IRepositories;

public interface IDomainReportRepository: IDomainReportRepositoryCustom
{
    // custom methods for only repository
    Task<IEnumerable<DomainReport>> GetReports(string domain, EDomainReportTimeframe timeFrame);
}

public interface IDomainReportRepositoryCustom
{
    // custom methods shared between repository and service
    Task AddReport(Guid domainId, Guid userId, EReportType reportType = EReportType.ConnectionIssue);
    Task<bool> CanReport(Guid domainId, Guid userId, EReportType reportType = EReportType.ConnectionIssue);
}