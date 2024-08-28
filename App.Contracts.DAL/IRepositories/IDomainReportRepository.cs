using App.Domain.Enums;
using App.DTO.Common;
using PublicV1 = App.DTO.Public.v1;
namespace App.Contracts.DAL.IRepositories;

public interface IDomainReportRepository: IDomainReportRepositoryCustom<DomainReport>
{
    // custom methods for only repository
}

public interface IDomainReportRepositoryCustom<TEntity>
{
    // custom methods shared between repository and service
    Task<IEnumerable<TEntity>> GetReports(string domain, EDomainReportTimeframe timeFrame);
    Task AddReport(Guid domainId, Guid userId, EReportType reportType = EReportType.ConnectionIssue);
    Task<bool> CanReport(Guid domainId, Guid userId, EReportType reportType = EReportType.ConnectionIssue);
}