using App.DAL.EF;
using App.DAL.EF.Seeding;
using App.Domain.Identity;
using Microsoft.AspNetCore.Identity;

namespace WebApp;

public static class SeedingRunner
{
    public static async Task SetupDb(IApplicationBuilder webApp, IConfiguration appConfiguration)
    {
        using var serviceScope = webApp.ApplicationServices
            .GetRequiredService<IServiceScopeFactory>()
            .CreateScope();
        await using var context = serviceScope.ServiceProvider.GetService<AppDbContext>();


        if (context == null)
        {
            throw new ApplicationException("Problem in services. Can't initialize Application DB Context");
        }

        using var userManager = serviceScope.ServiceProvider.GetService<UserManager<AppUser>>();
        using var roleManager = serviceScope.ServiceProvider.GetService<RoleManager<AppRole>>();

        if (userManager == null || roleManager == null)
        {
            throw new ApplicationException("Problem in services. Can't initialize UserManager or RoleManager");
        }

        var logger = serviceScope.ServiceProvider.GetService<ILogger<IApplicationBuilder>>();
        if (logger == null)
        {
            throw new ApplicationException("Problem in services. Can't initialize logger");
        }


        // wait for db connection
        var startedAt = DateTime.UtcNow;
        var isDbConnectable = context.Database.CanConnectAsync().Result;
        while (!isDbConnectable)
        {
            isDbConnectable = context.Database.CanConnectAsync().Result;
            if (!isDbConnectable && (DateTime.UtcNow - startedAt).Seconds > 5)
            {
                break;
            }
        }

        var isInMemoryDb = context.Database.ProviderName?.Contains("InMemory") ?? false;

        if (!isInMemoryDb && appConfiguration.GetValue<bool>("InitializeData:DropDatabase"))
        {
            logger.LogWarning("Dropping database");
            DbInitializer.DropDatabase(context);
        }

        if (!isInMemoryDb && appConfiguration.GetValue<bool>("InitializeData:MigrateDatabase"))
        {
            logger.LogInformation("Migrating database");
            DbInitializer.MigrateDatabase(context);
        }

        if (appConfiguration.GetValue<bool>("InitializeData:SeedIdentity"))
        {
            logger.LogInformation("Seeding identity");
            var adminPassword = appConfiguration.GetValue<string>("InitializeData:AdminPassword");
            await DbInitializer.SeedIdentity(userManager, roleManager, adminPassword!);
        }

        if (appConfiguration.GetValue<bool>("InitializeData:SeedData"))
        {
            logger.LogInformation("Seeding initial app data");
            await DbInitializer.SeedData(context);
        }
        
        if (appConfiguration.GetValue<bool>("InitializeData:SeedDevData"))
        {
            logger.LogInformation("Seeding random data for development");
            await DbInitializer.SeedDevData(context);
        }
    }
}