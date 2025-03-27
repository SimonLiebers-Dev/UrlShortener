# Unter https://aka.ms/customizecontainer erfahren Sie, wie Sie Ihren Debugcontainer anpassen und wie Visual Studio dieses Dockerfile verwendet, um Ihre Images für ein schnelleres Debuggen zu erstellen.

# Diese Stufe wird verwendet, wenn sie von VS im Schnellmodus ausgeführt wird (Standardeinstellung für Debugkonfiguration).
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# Diese Stufe wird zum Erstellen des Dienstprojekts verwendet.
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["UrlShortener.App.Blazor/UrlShortener.App.Blazor/UrlShortener.App.Blazor.csproj", "UrlShortener.App.Blazor/UrlShortener.App.Blazor/"]
COPY ["UrlShortener.App.Blazor/UrlShortener.App.Blazor.Client/UrlShortener.App.Blazor.Client.csproj", "UrlShortener.App.Blazor/UrlShortener.App.Blazor.Client/"]
COPY ["UrlShortener.App.Shared/UrlShortener.App.Shared.csproj", "UrlShortener.App.Shared/"]
RUN dotnet restore "./UrlShortener.App.Blazor/UrlShortener.App.Blazor/UrlShortener.App.Blazor.csproj"
COPY . .
WORKDIR "/src/UrlShortener.App.Blazor/UrlShortener.App.Blazor"
RUN dotnet build "./UrlShortener.App.Blazor.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Diese Stufe wird verwendet, um das Dienstprojekt zu veröffentlichen, das in die letzte Phase kopiert werden soll.
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./UrlShortener.App.Blazor.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Diese Stufe wird in der Produktion oder bei Ausführung von VS im regulären Modus verwendet (Standard, wenn die Debugkonfiguration nicht verwendet wird).
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "UrlShortener.App.Blazor.dll"]