using Base.Contracts.DAL;
using Dal = App.DTO.Private.DAL;
namespace App.Contracts.DAL.IRepositories;

public interface ICommentReactionRepository: IBaseRepository<Dal.CommentReaction>, ICommentReactionRepositoryCustom<Dal.CommentReaction>
{
    // custom methods for only repository
    Task<Domain.CommentReaction?> Get(Guid commentId, Guid userId);
}

public interface ICommentReactionRepositoryCustom<TEntity>
{
    // custom methods shared between repository and service
    Task<TEntity> Add(TEntity reaction);
    
    Task<TEntity> Update(TEntity reaction);
    
    Task Delete(Guid commentId, Guid userId);
}