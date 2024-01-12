using App.DTO.Public.v1;
using Base.Contracts.DAL;
using Dal = App.DTO.Private.DAL;
namespace App.Contracts.DAL.IRepositories;

public interface IGroupRepository: IBaseRepository<Dal.Group>, IGroupRepositoryCustom<Dal.Group>
{
    // custom methods for only repository
}

public interface IGroupRepositoryCustom<TEntity>
{
    // custom methods shared between repository and service
    Task<IEnumerable<TEntity>> GetAll(Guid userId);
}