using Base.Contracts.DAL;
using Microsoft.EntityFrameworkCore;

namespace Base.DAL.EF;

public class EfBaseUOW<TDbContext> : IBaseUOW
    where TDbContext : DbContext
{
    protected readonly TDbContext DbContext;

    public EfBaseUOW(TDbContext context)
    {
        DbContext = context;
    }
    
    public virtual async Task<int> SaveChangesAsync()
    {
        return await DbContext.SaveChangesAsync();
    }
}