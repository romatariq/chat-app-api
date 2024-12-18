FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# copy csproj and restore as distinct layers
COPY *.props .
COPY *.sln .
COPY App.Contracts.BLL/*.csproj ./App.Contracts.BLL/
COPY App.Contracts.DAL/*.csproj ./App.Contracts.DAL/
COPY App.BLL/*.csproj ./App.BLL/
COPY App.DAL.EF/*.csproj ./App.DAL.EF/
COPY App.Domain/*.csproj ./App.Domain/
COPY App.DTO.Common/*.csproj ./App.DTO.Common/
COPY App.DTO.Private/*.csproj ./App.DTO.Private/
COPY App.DTO.Public/*.csproj ./App.DTO.Public/
COPY App.Helpers/*.csproj ./App.Helpers/
COPY App.Mappers/*.csproj ./App.Mappers/
COPY WebApp/*.csproj ./WebApp/
COPY Base.Contracts/*.csproj ./Base.Contracts/
COPY Base.Contracts.BLL/*.csproj ./Base.Contracts.BLL/
COPY Base.Contracts.DAL/*.csproj ./Base.Contracts.DAL/
COPY Base.Contracts.Domain/*.csproj ./Base.Contracts.Domain/
COPY Base.BLL/*.csproj ./Base.BLL/
COPY Base.DAL/*.csproj ./Base.DAL/
COPY Base.DAL.EF/*.csproj ./Base.DAL.EF/
COPY Base.Domain/*.csproj ./Base.Domain/
COPY Base.Helpers/*.csproj ./Base.Helpers/

RUN dotnet restore

# copy everything else and build app
COPY App.Contracts.BLL/. ./App.Contracts.BLL/
COPY App.Contracts.DAL/. ./App.Contracts.DAL/
COPY App.BLL/. ./App.BLL/
COPY App.DAL.EF/. ./App.DAL.EF/
COPY App.Domain/. ./App.Domain/
COPY App.DTO.Common/. ./App.DTO.Common/
COPY App.DTO.Private/. ./App.DTO.Private/
COPY App.DTO.Public/. ./App.DTO.Public/
COPY App.Helpers/. ./App.Helpers/
COPY App.Mappers/. ./App.Mappers/
COPY WebApp/. ./WebApp/
COPY Base.Contracts/. ./Base.Contracts/
COPY Base.Contracts.BLL/. ./Base.Contracts.BLL/
COPY Base.Contracts.DAL/. ./Base.Contracts.DAL/
COPY Base.Contracts.Domain/. ./Base.Contracts.Domain/
COPY Base.BLL/. ./Base.BLL/
COPY Base.DAL/. ./Base.DAL/
COPY Base.DAL.EF/. ./Base.DAL.EF/
COPY Base.Domain/. ./Base.Domain/
COPY Base.Helpers/. ./Base.Helpers/

WORKDIR /src/WebApp
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
EXPOSE 8080

COPY --from=build /src/WebApp/out ./

ENTRYPOINT ["dotnet", "WebApp.dll"]