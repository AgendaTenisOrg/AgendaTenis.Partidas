#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/ports/AgendaTenis.Partidas.WebApi/AgendaTenis.Partidas.WebApi.csproj", "src/ports/AgendaTenis.Partidas.WebApi/"]
COPY ["src/core/AgendaTenis.Partidas.Core/AgendaTenis.Partidas.Core.csproj", "src/core/AgendaTenis.Partidas.Core/"]
RUN dotnet restore "./src/ports/AgendaTenis.Partidas.WebApi/AgendaTenis.Partidas.WebApi.csproj"
COPY . .
WORKDIR "/src/src/ports/AgendaTenis.Partidas.WebApi"
RUN dotnet build "./AgendaTenis.Partidas.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./AgendaTenis.Partidas.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AgendaTenis.Partidas.WebApi.dll"]