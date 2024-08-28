using App.Contracts.BLL.IServices;
using App.Contracts.DAL;
using App.Contracts.DAL.IRepositories;
using App.Mappers.AutoMappers.BLL;
using AutoMapper;
using Base.BLL;
using Bll = App.DTO.Private.BLL;
using Dal = App.DTO.Private.DAL;

namespace App.BLL.Services;

public class GroupService: BaseService<Dal.Group, Bll.Group, IGroupRepository>, IGroupService
{

    public GroupService(IAppUOW uow, IMapper mapper)
        : base(uow.GroupRepository, new GroupMapper(mapper))
    {
    }

    public async Task<IEnumerable<Bll.Group>> GetAll(Guid? userId)
    {
        var groups = await Repository.GetAll(userId);
        return groups.Select(Mapper.Map)!;
    }

    public async Task<bool> IsUserInGroup(Guid userId, Guid groupId)
    {
        return await Repository.IsUserInGroup(userId, groupId);
    }
}