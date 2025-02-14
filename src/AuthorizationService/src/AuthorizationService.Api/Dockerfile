FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/AuthorizationService/src/AuthorizationService.Api/AuthorizationService.Api.csproj", "src/AuthorizationService/src/AuthorizationService.Api/"]
COPY ["src/SharedService/src/MessagePublisherService/src/MessagePublisher/MessagePublisher.csproj", "src/SharedService/src/MessagePublisherService/src/MessagePublisher/"]
COPY ["src/AuthorizationService/src/AuthorizationService.Domain/AuthorizationService.Domain.csproj", "src/AuthorizationService/src/AuthorizationService.Domain/"]
COPY ["src/AuthorizationService/src/AuthorizationService.Infrastructure/AuthorizationService.Infrastructure.csproj", "src/AuthorizationService/src/AuthorizationService.Infrastructure/"]
COPY ["src/AuthorizationService/src/AuthorizationService.Application/AuthorizationService.Application.csproj", "src/AuthorizationService/src/AuthorizationService.Application/"]
COPY ["src/SharedService/src/CoreConfiguration/CoreConfiguration.csproj", "src/SharedService/src/CoreConfiguration/"]
RUN dotnet restore "src/AuthorizationService/src/AuthorizationService.Api/AuthorizationService.Api.csproj"
COPY . .
WORKDIR "/src/src/AuthorizationService/src/AuthorizationService.Api"
RUN dotnet build "AuthorizationService.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "AuthorizationService.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AuthorizationService.Api.dll"]
