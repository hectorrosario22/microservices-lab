# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY Auth.Api/Auth.Api.csproj Auth.Api/
COPY Events.Contracts/Events.Contracts.csproj Events.Contracts/

RUN dotnet restore Auth.Api/Auth.Api.csproj
COPY . .

WORKDIR /src/Auth.Api
RUN dotnet publish -c Release -o /app/publish

# Stage 2: Final
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:80

EXPOSE 80
ENTRYPOINT ["dotnet", "Auth.Api.dll"]