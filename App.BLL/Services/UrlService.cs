using App.Contracts.BLL.IServices;
using App.Contracts.DAL;

namespace App.BLL.Services;

public class UrlService: IUrlService
{
    protected IAppUOW Uow;

    public UrlService(IAppUOW uow)
    {
        Uow = uow;
    }

    public async Task<Guid> GetOrCreateDomainId(string domain)
    {
        return await Uow.UrlRepository.GetOrCreateDomainId(domain);
    }

    public async Task<Guid> GetOrCreateUrlId(Guid domainId, string? path, string? parameters)
    {
        return await Uow.UrlRepository.GetOrCreateUrlId(domainId, path, parameters);
    }
}