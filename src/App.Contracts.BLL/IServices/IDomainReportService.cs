using App.Contracts.DAL.IRepositories;
using App.DTO.Common;

namespace App.Contracts.BLL.IServices;

public interface IDomainReportService: IDomainReportRepositoryCustom
{
    // custom service methods
    Task<(IEnumerable<DomainReport> reports, string domain)> GetReports(string encodedUrl, EDomainReportTimeframe timeFrame);
}