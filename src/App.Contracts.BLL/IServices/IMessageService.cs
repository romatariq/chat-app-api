using App.Contracts.DAL.IRepositories;
using Base.Contracts.DAL;
using Bll = App.DTO.Private.BLL;

namespace App.Contracts.BLL.IServices;

public interface IMessageService: IBaseRepository<Bll.Message>, IMessageRepositoryCustom<Bll.Message>
{
    // add your custom service methods here
}