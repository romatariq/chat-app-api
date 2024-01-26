using App.Contracts.DAL.IRepositories;
using Base.Contracts.DAL;
using Bll = App.DTO.Private.BLL;

namespace App.Contracts.BLL.IServices;

public interface ICommentService: IBaseRepository<Bll.Comment>, ICommentRepositoryCustom<Bll.Comment>
{
    // add your custom service methods here
}