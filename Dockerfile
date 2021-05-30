FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app

ENV TZ="Asia/Kolkata"

RUN sed -i 's/DEFAULT@SECLEVEL=2/DEFAULT@SECLEVEL=1/g' /etc/ssl/openssl.cnf
RUN sed -i 's/DEFAULT@SECLEVEL=2/DEFAULT@SECLEVEL=1/g' /usr/lib/ssl/openssl.cnf

EXPOSE 6001

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ShopBridge.sln ./
COPY ShopBridge/*.csproj ./ShopBridge/
COPY ShopBridge.Common/*.csproj ShopBridge.Common/
COPY ShopBridge.Domain/*.csproj ShopBridge.Domain/
COPY ShopBridge.DTO/*.csproj ShopBridge.DTO/
COPY ShopBridge.Interface/ShopBridge.ProviderInterface/*.csproj ShopBridge.Interface/ShopBridge.ProviderInterface/
COPY ShopBridge.Interface/ShopBridge.RepositoryInterface/*.csproj ShopBridge.Interface/ShopBridge.RepositoryInterface/
COPY ShopBridge.Provider/*.csproj ShopBridge.Provider/
COPY ShopBridge.Repository/ShopBridge.CacheRepository/*.csproj ShopBridge.Repository/ShopBridge.CacheRepository/
COPY ShopBridge.Repository/ShopBridge.DBRepository/*.csproj ShopBridge.Repository/ShopBridge.DBRepository/

RUN dotnet restore
COPY . .
WORKDIR /src/ShopBridge
RUN dotnet publish -c Release -o /app

WORKDIR /src/ShopBridge.Common
RUN dotnet publish -c Release -o /app

WORKDIR /src/ShopBridge.Domain
RUN dotnet publish -c Release -o /app

WORKDIR /src/ShopBridge.DTO
RUN dotnet publish -c Release -o /app

WORKDIR /src/ShopBridge.Interface/ShopBridge.ProviderInterface
RUN dotnet publish -c Release -o /app

WORKDIR /src/ShopBridge.Interface/ShopBridge.RepositoryInterface
RUN dotnet publish -c Release -o /app

WORKDIR /src/ShopBridge.Provider
RUN dotnet publish -c Release -o /app

WORKDIR /src/ShopBridge.Repository/ShopBridge.CacheRepository
RUN dotnet publish -c Release -o /app

WORKDIR /src/ShopBridge.Repository/ShopBridge.DBRepository
RUN dotnet publish -c Release -o /app

FROM build AS publish
RUN dotnet publish -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app/ .
ENTRYPOINT ["dotnet", "ShopBridge.dll"]