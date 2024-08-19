using App.Contracts.BLL.IServices;
using App.Contracts.DAL;
using App.Domain.Enums;
using App.DTO.Common;
using App.DTO.Public.v1;

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
        return await Uow.DomainReportRepository.GetReports(domain, timeFrame);
    }

    public async Task AddReport(Guid domainId, Guid userId, EReportType reportType = EReportType.ConnectionIssue)
    {
        await Uow.DomainReportRepository.AddReport(domainId, userId, reportType);
    }
}