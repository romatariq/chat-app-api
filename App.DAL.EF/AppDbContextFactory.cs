using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace App.DAL.EF;

public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    
    public AppDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        optionsBuilder.UseNpgsql(Environment.GetEnvironmentVariable("ConnectionStrings__DefaultConnection"));

        return new AppDbContext(optionsBuilder.Options);
    }
}
