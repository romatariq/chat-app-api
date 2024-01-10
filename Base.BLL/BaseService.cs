using Base.Contracts;
using Base.Contracts.BLL;
using Base.Contracts.DAL;
using Base.Contracts.Domain;

namespace Base.BLL;

public class
    BaseService<TDalEntity, TBllEntity, TRepository> :
    BaseService<TDalEntity, TBllEntity, TRepository, Guid>, IService<TBllEntity>
    where TDalEntity : class, IDomainEntityId
    where TBllEntity : class, IDomainEntityId
    where TRepository : IBaseRepository<TDalEntity>
{
    public BaseService(TRepository repository, IMapper<TDalEntity, TBllEntity> mapper) : base(repository, mapper)
    {
    }
}

public class BaseService<TDalEntity, TBllEntity, TRepository, TKey> : IService<TBllEntity, TKey>
    where TDalEntity : class, IDomainEntityId<TKey>
    where TBllEntity : class, IDomainEntityId<TKey>
    where TRepository : IBaseRepository<TDalEntity, TKey>
    where TKey : struct, IEquatable<TKey>
{
    protected readonly TRepository Repository;
    protected readonly IMapper<TDalEntity, TBllEntity> Mapper;

    public BaseService(TRepository repository , IMapper<TDalEntity, TBllEntity> mapper)
    {
        Repository = repository;
        Mapper = mapper;
    }
}