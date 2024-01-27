namespace App.Contracts.DAL.IRepositories;

public interface IUrlRepository: IUrlRepositoryCustom
{
    // custom methods for only repository
    Task<Guid> GetOrCreateDomainId(string domain);
    Task<Guid> GetOrCreateUrlId(Guid domainId, string? path, string? parameters);
}

public interface IUrlRepositoryCustom
{
    // custom methods shared between repository and service
}