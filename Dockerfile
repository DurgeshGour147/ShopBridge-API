FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app

ENV TZ="Asia/Kolkata"

RUN sed -i 's/DEFAULT@SECLEVEL=2/DEFAULT@SECLEVEL=1/g' /etc/ssl/openssl.cnf
RUN sed -i 's/DEFAULT@SECLEVEL=2/DEFAULT@SECLEVEL=1/g' /usr/lib/ssl/openssl.cnf

EXPOSE 6001

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY Jewelry-Store.sln ./
COPY Jewelry-Store/*.csproj ./Jewelry-Store/
COPY Jewelry-Store.Common/*.csproj Jewelry-Store.Common/
COPY Jewelry-Store.Domain/*.csproj Jewelry-Store.Domain/
COPY Jewelry-Store.DTO/*.csproj Jewelry-Store.DTO/
COPY Jewelry-Store.Interface/Jewelry-Store.ProviderInterface/*.csproj Jewelry-Store.Interface/Jewelry-Store.ProviderInterface/
COPY Jewelry-Store.Interface/Jewelry-Store.RepositoryInterface/*.csproj Jewelry-Store.Interface/Jewelry-Store.RepositoryInterface/
COPY Jewelry-store.Provider/*.csproj Jewelry-store.Provider/
COPY Jewelry-Store.Repository/Jewelry-Store.CacheRepository/*.csproj Jewelry-Store.Repository/Jewelry-Store.CacheRepository/
COPY Jewelry-Store.Repository/Jewelry-Store.DBRepository/*.csproj Jewelry-Store.Repository/Jewelry-Store.DBRepository/

RUN dotnet restore
COPY . .
WORKDIR /src/Jewelry-Store
RUN dotnet publish -c Release -o /app

WORKDIR /src/Jewelry-Store.Common
RUN dotnet publish -c Release -o /app

WORKDIR /src/Jewelry-Store.Domain
RUN dotnet publish -c Release -o /app

WORKDIR /src/Jewelry-Store.DTO
RUN dotnet publish -c Release -o /app

WORKDIR /src/Jewelry-Store.Interface/Jewelry-Store.ProviderInterface
RUN dotnet publish -c Release -o /app

WORKDIR /src/Jewelry-Store.Interface/Jewelry-Store.RepositoryInterface
RUN dotnet publish -c Release -o /app

WORKDIR /src/Jewelry-store.Provider
RUN dotnet publish -c Release -o /app

WORKDIR /src/Jewelry-Store.Repository/Jewelry-Store.CacheRepository
RUN dotnet publish -c Release -o /app

WORKDIR /src/Jewelry-Store.Repository/Jewelry-Store.DBRepository
RUN dotnet publish -c Release -o /app

FROM build AS publish
RUN dotnet publish -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app/ .
ENTRYPOINT ["dotnet", "Jewelry-Store.dll"]