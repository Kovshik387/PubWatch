FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/ExchangeCacheService/src/ExchangeCacheService.Api/ExchangeCacheService.Api.csproj", "src/ExchangeCacheService/src/ExchangeCacheService.Api/"]
RUN dotnet restore "src/ExchangeCacheService/src/ExchangeCacheService.Api/ExchangeCacheService.Api.csproj"
COPY . .
WORKDIR "/src/src/ExchangeCacheService/src/ExchangeCacheService.Api"
RUN dotnet build "ExchangeCacheService.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "ExchangeCacheService.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ExchangeCacheService.Api.dll"]
