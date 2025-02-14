FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

ENV DOTNET_SYSTEM_NET_HTTP_SOCKETSHTTPHANDLER_HTTP2SUPPORT=true

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/AccountService/src/AccountService.Api/AccountService.Api.csproj", "src/AccountService/src/AccountService.Api/"]
COPY ["src/AccountService/src/AccountService.Domain/AccountService.Domain.csproj", "src/AccountService/src/AccountService.Domain/"]
COPY ["src/AccountService/src/AccountService.Infrastructure/AccountService.Infrastructure.csproj", "src/AccountService/src/AccountService.Infrastructure/"]
COPY ["src/AccountService/src/AccountService.Application/AccountService.Application.csproj", "src/AccountService/src/AccountService.Application/"]
COPY ["src/SharedService/src/CoreConfiguration/CoreConfiguration.csproj", "src/SharedService/src/CoreConfiguration/"]
RUN dotnet restore "src/AccountService/src/AccountService.Api/AccountService.Api.csproj"
COPY . .
WORKDIR "/src/src/AccountService/src/AccountService.Api"
RUN dotnet build "AccountService.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "AccountService.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AccountService.Api.dll"]
