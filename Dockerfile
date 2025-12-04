# 1. Base Stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# 2. Build Stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["ShopHub.csproj", "."]
RUN dotnet restore "./ShopHub.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "./ShopHub.csproj" -c $BUILD_CONFIGURATION -o /app/build

# 3. Publish Stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./ShopHub.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# 4. Final Stage (Production)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# FORCE PORT 8080 FOR CLOUD DEPLOYMENT
ENV ASPNETCORE_HTTP_PORTS=8080

ENTRYPOINT ["dotnet", "ShopHub.dll"]