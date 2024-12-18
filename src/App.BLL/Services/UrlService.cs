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
        var urlParts = UrlHelpers.ParseEncodedUrl(encodedUrl);

        var domainId = await Uow.UrlRepository.GetOrCreateDomainId(urlParts.domain);
        var urlId = await Uow.UrlRepository.GetOrCreateUrlId(domainId, urlParts.path, urlParts.parameters);

        return urlId;
    }

    public async Task<Guid> GetOrCreateDomainId(string encodedUrl)
    {
        var domain = UrlHelpers.ParseEncodedUrl(encodedUrl).domain;
        return await Uow.UrlRepository.GetOrCreateDomainId(domain);
    }
}