using App.Contracts.DAL.IRepositories;
using App.DTO.Common;
using Base.Contracts.DAL;
using Bll = App.DTO.Private.BLL;

namespace App.Contracts.BLL.IServices;

public interface ICommentService: IBaseRepository<Bll.Comment>, ICommentRepositoryCustom<Bll.Comment>
{
    // add your custom service methods here
    Task<(IEnumerable<Bll.Comment> comments, int totalPageCount)> GetAll(Guid? userId, string encodedUrl, ESort sort, int pageNr, int pageSize);
}