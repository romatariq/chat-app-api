using App.Contracts.DAL.IRepositories;
using App.DTO.Common;
using PublicV1 = App.DTO.Public.v1;

namespace App.Contracts.BLL.IServices;

public interface IDomainReportService: IDomainReportRepositoryCustom<DomainReport>
{
    // custom service methods
}