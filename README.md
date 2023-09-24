# Chat app backend

## Migrations
~~~bash
# install or update tools
dotnet tool install --global dotnet-ef
dotnet tool update --global dotnet-ef

# create migration
dotnet ef migrations add Initial --project App.DAL.EF --startup-project WebApp --context AppDbContext

# apply migration
dotnet ef database update --project App.DAL.EF --startup-project WebApp --context AppDbContext
~~~