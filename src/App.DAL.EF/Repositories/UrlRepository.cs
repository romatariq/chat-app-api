using App.Contracts.DAL.IRepositories;
using App.Domain;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF.Repositories;

public class UrlRepository: EfBaseRepository<Domain.Url, AppDbContext>, IUrlRepository
{
    public UrlRepository(AppDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<Guid> GetOrCreateDomainId(string domain)
    {
        var domainId = await DbContext.WebDomains
            .Where(d => d.Name == domain)
            .Select(d => (Guid?) d.Id)
            .SingleOrDefaultAsync();
        
        domainId ??= (await DbContext.WebDomains
            .AddAsync(new WebDomain
            {
                Name = domain
            })).Entity.Id;

        return domainId.Value;
    }

    public async Task<Guid> GetOrCreateUrlId(Guid domainId, string? path, string? parameters)
    {
        var urlId = await DbSet
            .Where(u => 
                u.WebDomainId == domainId &&
                u.Path == path &&
                u.Params == parameters)
            .Select(u => (Guid?) u.Id)
            .SingleOrDefaultAsync();
        
        urlId ??= (await DbSet
            .AddAsync(new Url
            {
                WebDomainId = domainId,
                Path = path,
                Params = parameters
            })).Entity.Id;

        return urlId.Value;
    }
}