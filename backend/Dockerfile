FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy solution and project files
COPY ["Tralivali.sln", "./"]
COPY ["Tralivali.API/Tralivali.API.csproj", "Tralivali.API/"]
COPY ["Tralivali.Core/Tralivali.Core.csproj", "Tralivali.Core/"]
COPY ["Tralivali.Infrastructure/Tralivali.Infrastructure.csproj", "Tralivali.Infrastructure/"]

# Restore dependencies
RUN dotnet restore "Tralivali.sln"

# Copy everything else and build
COPY . .
WORKDIR "/src/Tralivali.API"
RUN dotnet build "Tralivali.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Tralivali.API.csproj" -c Release -o /app/publish

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Tralivali.API.dll"]
