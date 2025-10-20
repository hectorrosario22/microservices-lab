# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["*.csproj", "./"]
RUN dotnet restore
COPY . .
RUN dotnet publish -c Release -o /app/publish

# Stage 2: Final
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

# Worker Services no necesitan exponer un puerto HTTP, pero deben ejecutarse.
# Usamos la imagen aspnet para garantizar que todas las librerías necesarias de .NET estén disponibles.

ENTRYPOINT ["dotnet", "Notification.Worker.dll"]