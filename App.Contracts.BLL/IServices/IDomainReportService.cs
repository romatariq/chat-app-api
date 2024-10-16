using App.Contracts.DAL.IRepositories;
using App.DTO.Common;
using App.DTO.Public.v1;
using PublicV1 = App.DTO.Public.v1;

namespace App.Contracts.BLL.IServices;

public interface IDomainReportService: IDomainReportRepositoryCustom
{
    // custom service methods
    Task<DomainReports> GetReports(string encodedUrl, EDomainReportTimeframe timeFrame);
}