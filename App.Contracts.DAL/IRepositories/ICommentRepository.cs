using App.DTO.Private.Shared;
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
    Task<(IEnumerable<TEntity> comments, int totalPageCount)> GetAll(GetAllCommentsParameters parameters);
    
    Task<TEntity> Add(Guid urlId, Guid groupId, Guid userId, string text, string username);
}