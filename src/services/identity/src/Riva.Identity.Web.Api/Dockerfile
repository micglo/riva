FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-nanoserver-1903 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-nanoserver-1903 AS build
WORKDIR /src

COPY ["../Riva.sln", "Riva.sln"]

COPY ["apiGateways/web/src/Riva.Web.Api.Gateway/Riva.Web.Api.Gateway.csproj", "apiGateways/web/src/Riva.Web.Api.Gateway/Riva.Web.Api.Gateway.csproj"]

COPY ["buildingBlocks/src/Riva.BuildingBlocks.Core/Riva.BuildingBlocks.Core.csproj", "buildingBlocks/src/Riva.BuildingBlocks.Core/Riva.BuildingBlocks.Core.csproj"]
COPY ["buildingBlocks/src/Riva.BuildingBlocks.Domain/Riva.BuildingBlocks.Domain.csproj", "buildingBlocks/src/Riva.BuildingBlocks.Domain/Riva.BuildingBlocks.Domain.csproj"]
COPY ["buildingBlocks/src/Riva.BuildingBlocks.Infrastructure/Riva.BuildingBlocks.Infrastructure.csproj", "buildingBlocks/src/Riva.BuildingBlocks.Infrastructure/Riva.BuildingBlocks.Infrastructure.csproj"]
COPY ["buildingBlocks/src/Riva.BuildingBlocks.WebApi/Riva.BuildingBlocks.WebApi.csproj", "buildingBlocks/src/Riva.BuildingBlocks.WebApi/Riva.BuildingBlocks.WebApi.csproj"]

COPY ["services/administrativeDivisions/src/Riva.AdministrativeDivisions.Core/Riva.AdministrativeDivisions.Core.csproj", "services/administrativeDivisions/src/Riva.AdministrativeDivisions.Core/Riva.AdministrativeDivisions.Core.csproj"]
COPY ["services/administrativeDivisions/src/Riva.AdministrativeDivisions.Domain/Riva.AdministrativeDivisions.Domain.csproj", "services/administrativeDivisions/src/Riva.AdministrativeDivisions.Domain/Riva.AdministrativeDivisions.Domain.csproj"]
COPY ["services/administrativeDivisions/src/Riva.AdministrativeDivisions.Infrastructure/Riva.AdministrativeDivisions.Infrastructure.csproj", "services/administrativeDivisions/src/Riva.AdministrativeDivisions.Infrastructure/Riva.AdministrativeDivisions.Infrastructure.csproj"]
COPY ["services/administrativeDivisions/src/Riva.AdministrativeDivisions.Web.Api/Riva.AdministrativeDivisions.Web.Api.csproj", "services/administrativeDivisions/src/Riva.AdministrativeDivisions.Web.Api/Riva.AdministrativeDivisions.Web.Api.csproj"]

COPY ["services/announcements/src/Riva.Announcements.Core/Riva.Announcements.Core.csproj", "services/announcements/src/Riva.Announcements.Core/Riva.Announcements.Core.csproj"]
COPY ["services/announcements/src/Riva.Announcements.Domain/Riva.Announcements.Domain.csproj", "services/announcements/src/Riva.Announcements.Domain/Riva.Announcements.Domain.csproj"]
COPY ["services/announcements/src/Riva.Announcements.Infrastructure/Riva.Announcements.Infrastructure.csproj", "services/announcements/src/Riva.Announcements.Infrastructure/Riva.Announcements.Infrastructure.csproj"]
COPY ["services/announcements/src/Riva.Announcements.Web.Api/Riva.Announcements.Web.Api.csproj", "services/announcements/src/Riva.Announcements.Web.Api/Riva.Announcements.Web.Api.csproj"]

COPY ["services/identity/src/Riva.Identity.Core/Riva.Identity.Core.csproj", "services/identity/src/Riva.Identity.Core/Riva.Identity.Core.csproj"]
COPY ["services/identity/src/Riva.Identity.Domain/Riva.Identity.Domain.csproj", "services/identity/src/Riva.Identity.Domain/Riva.Identity.Domain.csproj"]
COPY ["services/identity/src/Riva.Identity.Infrastructure/Riva.Identity.Infrastructure.csproj", "services/identity/src/Riva.Identity.Infrastructure/Riva.Identity.Infrastructure.csproj"]
COPY ["services/identity/src/Riva.Identity.Web/Riva.Identity.Web.csproj", "services/identity/src/Riva.Identity.Web/Riva.Identity.Web.csproj"]
COPY ["services/identity/src/Riva.Identity.Web.Api/Riva.Identity.Web.Api.csproj", "services/identity/src/Riva.Identity.Web.Api/Riva.Identity.Web.Api.csproj"]

COPY ["services/signalR/src/Riva.SignalR/Riva.SignalR.csproj", "services/signalR/src/Riva.SignalR/Riva.SignalR.csproj"]

COPY ["services/users/src/Riva.Users.Core/Riva.Users.Core.csproj", "services/users/src/Riva.Users.Core/Riva.Users.Core.csproj"]
COPY ["services/users/src/Riva.Users.Domain/Riva.Users.Domain.csproj", "services/users/src/Riva.Users.Domain/Riva.Users.Domain.csproj"]
COPY ["services/users/src/Riva.Users.Infrastructure/Riva.Users.Infrastructure.csproj", "services/users/src/Riva.Users.Infrastructure/Riva.Users.Infrastructure.csproj"]
COPY ["services/users/src/Riva.Users.Web.Api/Riva.Users.Web.Api.csproj", "services/users/src/Riva.Users.Web.Api/Riva.Users.Web.Api.csproj"]

COPY ["docker-compose.dcproj", "docker-compose.dcproj"]

RUN dotnet restore "Riva.csproj"

COPY . .
WORKDIR "/src/services/identity/src/Riva.Identity.Web.Api"
RUN dotnet publish "Riva.Identity.Web.Api.csproj" --no-restore -c Release -o /app

FROM build AS publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Riva.Identity.Web.Api.dll"]