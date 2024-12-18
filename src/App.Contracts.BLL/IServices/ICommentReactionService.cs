using App.Contracts.DAL.IRepositories;
using Base.Contracts.DAL;
using Bll = App.DTO.Private.BLL;

namespace App.Contracts.BLL.IServices;

public interface ICommentReactionService: IBaseRepository<Bll.CommentReaction>, ICommentReactionRepositoryCustom<Bll.CommentReaction>
{
    // add your custom service methods here
}