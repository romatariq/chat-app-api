using App.Contracts.DAL.IRepositories;
using Base.Contracts.DAL;
using Bll = App.DTO.Private.BLL;

namespace App.Contracts.BLL.IServices;

public interface IGroupService: IBaseRepository<Bll.Group>, IGroupRepositoryCustom<Bll.Group>
{
    // add your custom service methods here
}