using App.Contracts.DAL.IRepositories;
using Base.Contracts.DAL;
using Bll = App.DTO.Private.BLL;

namespace App.Contracts.BLL.IServices;

public interface IUrlService: IUrlRepositoryCustom
{
    // add your custom service methods here
    Task<Guid> GetOrCreateUrlId(string encodedUrl);
}