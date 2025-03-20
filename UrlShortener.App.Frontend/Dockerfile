# Build stage for the frontend (Blazor WebAssembly)
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build-env
WORKDIR /app

# Copying the frontend project
COPY ["UrlShortener.App.Frontend/UrlShortener.App.Frontend.csproj", "UrlShortener.App.Frontend/"]
COPY ["UrlShortener.App.Shared/UrlShortener.App.Shared.csproj", "UrlShortener.App.Shared/"]

# Restore dependencies for the frontend and shared projects
RUN dotnet restore "./UrlShortener.App.Frontend/UrlShortener.App.Frontend.csproj"

# Copy all the files to the container
COPY . ./
WORKDIR "/app/UrlShortener.App.Frontend"

# Publish the frontend Blazor WebAssembly app
RUN dotnet publish -c Release -o /app/output

# Final stage to serve the Blazor WebAssembly app with NGINX
FROM nginx:alpine
WORKDIR /var/www/web

# Copy the published files from the build stage
COPY --from=build-env /app/output/wwwroot ./

# Copy the NGINX configuration
COPY UrlShortener.App.Frontend/nginx.conf /etc/nginx/nginx.conf

EXPOSE 80