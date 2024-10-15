using App.Contracts.BLL.IServices;
using App.Contracts.DAL;
using App.Helpers;

namespace App.BLL.Services;

public class UrlService: IUrlService
{
    protected IAppUOW Uow;

    public UrlService(IAppUOW uow)
    {
        Uow = uow;
    }

    public async Task<Guid> GetOrCreateUrlId(string encodedUrl)
    {
        var (domain, path, parameters) = UrlHelpers.ParseEncodedUrl(encodedUrl);

        var domainId = await Uow.UrlRepository.GetOrCreateDomainId(domain);
        var urlId = await Uow.UrlRepository.GetOrCreateUrlId(domainId, path, parameters);

        return urlId;
    }

    public async Task<Guid> GetOrCreateDomainId(string domain)
    {
        return await Uow.UrlRepository.GetOrCreateDomainId(domain);
    }
}