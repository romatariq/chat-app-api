using App.DTO.Public.v1;
using Base.Contracts.DAL;
using Dal = App.DTO.Private.DAL;
namespace App.Contracts.DAL.IRepositories;

public interface ICommentRepository: IBaseRepository<Dal.Comment>, ICommentRepositoryCustom<Dal.Comment>
{
    // custom methods for only repository
}

public interface ICommentRepositoryCustom<TEntity>
{
    // custom methods shared between repository and service
    Task<(IEnumerable<TEntity> comments, int totalPageCount)> GetAll(Guid groupId, Guid userId, string domain, string? path, string? parameters, ESort sort, int pageNr, int pageSize);
    
    Task<TEntity> Add(Guid urlId, Guid groupId, Guid userId, string text, string username);
}