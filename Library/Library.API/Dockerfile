FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER root 
WORKDIR /app
RUN mkdir -p /app/Uploads && chmod -R 777 /app/Uploads
USER app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Library.API/Library.API.csproj", "Library.API/"]
COPY ["Library.Application/Library.Application.csproj", "Library.Application/"]
COPY ["Library.Core/Library.Core.csproj", "Library.Core/"]
COPY ["Library.DataAccess/Library.DataAccess.csproj", "Library.DataAccess/"]

RUN dotnet restore "./Library.API/Library.API.csproj"
COPY . .
WORKDIR "/src/Library.API"
RUN dotnet build "./Library.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Library.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

COPY "./Library.API/Uploads" "./Uploads"

ENTRYPOINT ["dotnet", "./Library.API.dll"]