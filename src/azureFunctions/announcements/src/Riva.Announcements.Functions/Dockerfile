FROM mcr.microsoft.com/azure-functions/dotnet:3.0-nanoserver-1809 AS base
WORKDIR /home/site/wwwroot
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-nanoserver-1903 AS build
WORKDIR /src

COPY ["azureFunctions/announcements/src/Riva.Announcements.Functions/Riva.Announcements.Functions.csproj", "azureFunctions/announcements/src/Riva.Announcements.Functions/Riva.Announcements.Functions.csproj"]

RUN dotnet restore "Riva.Announcements.Functions.csproj"

COPY . .
WORKDIR "/src/azureFunctions/announcements/src/Riva.Announcements.Functions"
RUN dotnet publish "Riva.Announcements.Functions.csproj" -c Release -o /app

FROM base AS final
WORKDIR /home/site/wwwroot
COPY --from=publish /app .
ENV AzureWebJobsScriptRoot=/home/site/wwwroot \
    AzureFunctionsJobHost__Logging__Console__IsEnabled=true
