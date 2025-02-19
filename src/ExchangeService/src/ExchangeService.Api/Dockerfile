FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER root
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

RUN apt-get update && apt-get install -y curl ca-certificates && update-ca-certificates

ENV LANG=ru_RU.UTF-8
ENV LANGUAGE=ru_RU:ru
ENV LC_ALL=ru_RU.UTF-8

USER app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/ExchangeService/src/ExchangeService.Api/ExchangeService.Api.csproj", "src/ExchangeService/src/ExchangeService.Api/"]
COPY ["src/SharedService/src/CoreConfiguration/CoreConfiguration.csproj", "src/SharedService/src/CoreConfiguration/"]
COPY ["src/ExchangeService/src/ExchangeService.Infrastructure/ExchangeService.Infrastructure.csproj", "src/ExchangeService/src/ExchangeService.Infrastructure/"]
COPY ["src/ExchangeService/src/ExchangeService.Application/ExchangeService.Application.csproj", "src/ExchangeService/src/ExchangeService.Application/"]
COPY ["src/ExchangeService/src/ExchangeService.Domain/ExchangeService.Domain.csproj", "src/ExchangeService/src/ExchangeService.Domain/"]
RUN dotnet restore "src/ExchangeService/src/ExchangeService.Api/ExchangeService.Api.csproj"
COPY . .
WORKDIR "/src/src/ExchangeService/src/ExchangeService.Api"
RUN dotnet build "ExchangeService.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "ExchangeService.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ExchangeService.Api.dll"]
