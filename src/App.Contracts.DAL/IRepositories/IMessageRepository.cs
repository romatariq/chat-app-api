using Base.Contracts.DAL;
using Dal = App.DTO.Private.DAL;
namespace App.Contracts.DAL.IRepositories;

public interface IMessageRepository: IBaseRepository<Dal.Message>, IMessageRepositoryCustom<Dal.Message>
{
    // custom methods for only repository
}

public interface IMessageRepositoryCustom<TEntity>
{
    // custom methods shared between repository and service
    Task<TEntity> Add(string message, Guid urlId, Guid userId, string username);
    Task<IEnumerable<TEntity>> GetPreviousMessages(Guid urlId);
}