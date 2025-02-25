﻿FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/StorageService/src/StorageService.Api/StorageService.Api.csproj", "src/StorageService/src/StorageService.Api/"]
COPY ["src/SharedService/src/CoreConfiguration/CoreConfiguration.csproj", "src/SharedService/src/CoreConfiguration/"]
COPY ["src/StorageService/src/StorageService.Infrastructure/StorageService.Infrastructure.csproj", "src/StorageService/src/StorageService.Infrastructure/"]
COPY ["src/StorageService/src/StorageService.Application/StorageService.Application.csproj", "src/StorageService/src/StorageService.Application/"]
RUN dotnet restore "src/StorageService/src/StorageService.Api/StorageService.Api.csproj"
COPY . .
WORKDIR "/src/src/StorageService/src/StorageService.Api"
RUN dotnet build "StorageService.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "StorageService.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "StorageService.Api.dll"]
