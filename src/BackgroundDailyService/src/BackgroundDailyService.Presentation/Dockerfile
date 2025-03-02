FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $root
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

RUN apt-get update && apt-get install -y locales \
    && locale-gen ru_RU.UTF-8 \
    && apt-get clean

ENV LANG=ru_RU.UTF-8
ENV LANGUAGE=ru_RU:ru
ENV LC_ALL=ru_RU.UTF-8

USER $APP_UID

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/BackgroundDailyService/src/BackgroundDailyService.Presentation/BackgroundDailyService.Presentation.csproj", "src/BackgroundDailyService/src/BackgroundDailyService.Presentation/"]
COPY ["src/BackgroundDailyService/src/BackgroundDailyService.Domain/BackgroundDailyService.Domain.csproj", "src/BackgroundDailyService/src/BackgroundDailyService.Domain/"]
COPY ["src/BackgroundDailyService/src/BackgroundDailyService.Application/BackgroundDailyService.Application.csproj", "src/BackgroundDailyService/src/BackgroundDailyService.Application/"]
COPY ["src/SharedService/src/MessagePublisherService/src/MessagePublisher/MessagePublisher.csproj", "src/SharedService/src/MessagePublisherService/src/MessagePublisher/"]
COPY ["src/BackgroundDailyService/src/BackgroundDailyService.Infrastructure/BackgroundDailyService.Infrastructure.csproj", "src/BackgroundDailyService/src/BackgroundDailyService.Infrastructure/"]
RUN dotnet restore "src/BackgroundDailyService/src/BackgroundDailyService.Presentation/BackgroundDailyService.Presentation.csproj"
COPY . .
WORKDIR "/src/src/BackgroundDailyService/src/BackgroundDailyService.Presentation"
RUN dotnet build "BackgroundDailyService.Presentation.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "BackgroundDailyService.Presentation.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BackgroundDailyService.Presentation.dll"]
