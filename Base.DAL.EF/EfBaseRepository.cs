using Base.Contracts.DAL;
using Base.Contracts.Domain;
using Microsoft.EntityFrameworkCore;

namespace Base.DAL.EF;

public class EfBaseRepository<TEntity, TDbContext> : EfBaseRepository<TEntity, Guid, TDbContext>, IBaseRepository<TEntity>
    where TEntity: class, IDomainEntityId
    where TDbContext: DbContext
{
    public EfBaseRepository(TDbContext dbContext) : base(dbContext)
    {
    }
}

public class EfBaseRepository<TEntity, TKey, TDbContext> : IBaseRepository<TEntity, TKey>
    where TEntity : class, IDomainEntityId<TKey>
    where TKey: struct, IEquatable<TKey>
    where TDbContext : DbContext
{
    
    protected TDbContext DbContext;
    protected DbSet<TEntity> DbSet;
    
    public EfBaseRepository(TDbContext dbContext)
    {
        DbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        DbSet = dbContext.Set<TEntity>();
    }
}