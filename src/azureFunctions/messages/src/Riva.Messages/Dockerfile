FROM mcr.microsoft.com/azure-functions/dotnet:3.0-nanoserver-1809 AS base
WORKDIR /home/site/wwwroot
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-nanoserver-1903 AS build
WORKDIR /src

COPY ["azureFunctions/messages/src/Riva.Messages/Riva.Messages.csproj", "azureFunctions/messages/src/Riva.Messages/Riva.Messages.csproj"]
COPY ["buildingBlocks/src/Riva.BuildingBlocks.Core/Riva.BuildingBlocks.Core.csproj", "buildingBlocks/src/Riva.BuildingBlocks.Core/Riva.BuildingBlocks.Core.csproj"]
COPY ["buildingBlocks/src/Riva.BuildingBlocks.Domain/Riva.BuildingBlocks.Domain.csproj", "buildingBlocks/src/Riva.BuildingBlocks.Domain/Riva.BuildingBlocks.Domain.csproj"]
COPY ["buildingBlocks/src/Riva.BuildingBlocks.Infrastructure/Riva.BuildingBlocks.Infrastructure.csproj", "buildingBlocks/src/Riva.BuildingBlocks.Infrastructure/Riva.BuildingBlocks.Infrastructure.csproj"]

RUN dotnet restore "Riva.Messages.csproj"

COPY . .
WORKDIR "/src/azureFunctions/messages/src/Riva.Messages"
RUN dotnet publish "Riva.Messages.csproj" -c Release -o /app

FROM base AS final
WORKDIR /home/site/wwwroot
COPY --from=publish /app .
ENV AzureWebJobsScriptRoot=/home/site/wwwroot \
    AzureFunctionsJobHost__Logging__Console__IsEnabled=true