FROM mcr.microsoft.com/azure-functions/dotnet:3.0-nanoserver-1809 AS base
WORKDIR /home/site/wwwroot
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-nanoserver-1903 AS build
WORKDIR /src

COPY ["azureFunctions/announcementPreferences/src/Riva.AnnouncementPreferences.Core/Riva.AnnouncementPreferences.Core.csproj", "azureFunctions/announcementPreferences/src/Riva.AnnouncementPreferences.Core/Riva.AnnouncementPreferences.Core.csproj"]
COPY ["azureFunctions/announcementPreferences/src/Riva.AnnouncementPreferences.Functions/Riva.AnnouncementPreferences.Functions.csproj", "azureFunctions/announcementPreferences/src/Riva.AnnouncementPreferences.Functions/Riva.AnnouncementPreferences.Functions.csproj"]
COPY ["buildingBlocks/src/Riva.BuildingBlocks.Core/Riva.BuildingBlocks.Core.csproj", "buildingBlocks/src/Riva.BuildingBlocks.Core/Riva.BuildingBlocks.Core.csproj"]
COPY ["buildingBlocks/src/Riva.BuildingBlocks.Domain/Riva.BuildingBlocks.Domain.csproj", "buildingBlocks/src/Riva.BuildingBlocks.Domain/Riva.BuildingBlocks.Domain.csproj"]
COPY ["buildingBlocks/src/Riva.BuildingBlocks.Infrastructure/Riva.BuildingBlocks.Infrastructure.csproj", "buildingBlocks/src/Riva.BuildingBlocks.Infrastructure/Riva.BuildingBlocks.Infrastructure.csproj"]

RUN dotnet restore "Riva.AnnouncementPreferences.Functions.csproj"

COPY . .
WORKDIR "/src/azureFunctions/announcementPreferences/src/Riva.AnnouncementPreferences.Functions"
RUN dotnet publish "Riva.AnnouncementPreferences.Functions.csproj" -c Release -o /app

FROM base AS final
WORKDIR /home/site/wwwroot
COPY --from=publish /app .
ENV AzureWebJobsScriptRoot=/home/site/wwwroot \
    AzureFunctionsJobHost__Logging__Console__IsEnabled=true