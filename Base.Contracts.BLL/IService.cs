using Base.Contracts.DAL;
using Base.Contracts.Domain;

namespace Base.Contracts.BLL;

public interface IService<TEntity> : IBaseRepository<TEntity>, IService<TEntity, Guid>
    where TEntity : class, IDomainEntityId
{
}

public interface IService<TEntity, TKey> : IBaseRepository<TEntity, TKey>
    where TEntity : class, IDomainEntityId<TKey>
    where TKey : struct, IEquatable<TKey>
{
}