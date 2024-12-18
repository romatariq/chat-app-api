using Base.Contracts.Domain;

namespace Base.Contracts.DAL;

public interface IBaseRepository<TEntity> : IBaseRepository<TEntity, Guid>
    where TEntity: class, IDomainEntityId<Guid>
{
}

public interface IBaseRepository<TEntity, in TKey>
    where TEntity: class, IDomainEntityId<TKey>
    where TKey: struct, IEquatable<TKey>
{
}