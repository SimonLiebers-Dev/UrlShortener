FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["UrlShortener.App.Backend/UrlShortener.App.Backend.csproj", "UrlShortener.App.Backend/"]
COPY ["UrlShortener.App.Shared/UrlShortener.App.Shared.csproj", "UrlShortener.App.Shared/"]

RUN dotnet restore "./UrlShortener.App.Backend/UrlShortener.App.Backend.csproj"

COPY . .
WORKDIR "/src/UrlShortener.App.Backend"
RUN dotnet build "./UrlShortener.App.Backend.csproj" -c "$BUILD_CONFIGURATION" -o /app/build

# Diese Stufe wird verwendet, um das Dienstprojekt zu veröffentlichen, das in die letzte Phase kopiert werden soll.
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./UrlShortener.App.Backend.csproj" -c "$BUILD_CONFIGURATION" -o /app/publish /p:UseAppHost=false

# Diese Stufe wird in der Produktion oder bei Ausführung von VS im regulären Modus verwendet (Standard, wenn die Debugkonfiguration nicht verwendet wird).
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "UrlShortener.App.Backend.dll"]