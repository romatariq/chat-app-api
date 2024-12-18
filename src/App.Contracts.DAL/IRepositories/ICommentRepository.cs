using App.DTO.Common;
using Base.Contracts.DAL;
using Dal = App.DTO.Private.DAL;
namespace App.Contracts.DAL.IRepositories;

public interface ICommentRepository: IBaseRepository<Dal.Comment>, ICommentRepositoryCustom<Dal.Comment>
{
    // custom methods for only repository
    Task<(IEnumerable<Dal.Comment> comments, int totalPageCount)> GetAll(Guid? userId, string domain, string? path, string? parameters, ESort sort, int pageNr, int pageSize);
}

public interface ICommentRepositoryCustom<TEntity>
{
    // custom methods shared between repository and service
    Task<(IEnumerable<TEntity> comments, int totalPageCount)> GetAllReplies(Guid parentCommentId, Guid? userId, ESort sort, int pageSize, int pageNr);

    Task<TEntity> Add(Guid urlId, Guid userId, string text);

    Task<TEntity> AddReply(Guid parentCommentId, Guid replyToCommentId, Guid userId, string text);
}