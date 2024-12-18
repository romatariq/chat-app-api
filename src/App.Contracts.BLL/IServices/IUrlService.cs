using App.Contracts.DAL.IRepositories;

namespace App.Contracts.BLL.IServices;

public interface IUrlService: IUrlRepositoryCustom
{
    // add your custom service methods here
    Task<Guid> GetOrCreateUrlId(string encodedUrl);
    Task<Guid> GetOrCreateDomainId(string encodedUrl);
}