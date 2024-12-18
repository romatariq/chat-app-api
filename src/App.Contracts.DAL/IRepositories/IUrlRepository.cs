namespace App.Contracts.DAL.IRepositories;

public interface IUrlRepository: IUrlRepositoryCustom
{
    // custom methods for only repository
    Task<Guid> GetOrCreateUrlId(Guid domainId, string? path, string? parameters);
    Task<Guid> GetOrCreateDomainId(string domain);
}

public interface IUrlRepositoryCustom
{
    // shared between repository and service
}