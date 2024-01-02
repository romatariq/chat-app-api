using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace App.DAL.EF;

// To run dotnet ef database update from cli
public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    // Hard coded because cannot access env
    private const string ConnectionString = "Host=localhost:5445;Database=chat-app-db;Username=postgres;Password=postgres";
    
    public AppDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        optionsBuilder.UseNpgsql(ConnectionString);

        return new AppDbContext(optionsBuilder.Options);
    }
}
