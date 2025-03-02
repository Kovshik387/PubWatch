FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER root
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
COPY ["src/MessageService/src/MessageService.Api/MessageService.Api.csproj", "src/MessageService/src/MessageService.Api/"]
RUN dotnet restore "src/MessageService/src/MessageService.Api/MessageService.Api.csproj"
COPY . .
WORKDIR "/src/src/MessageService/src/MessageService.Api"
RUN dotnet build "MessageService.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "MessageService.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MessageService.Api.dll"]
