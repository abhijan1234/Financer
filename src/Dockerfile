#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /source

COPY ["Financer.API/Financer.API.csproj", "Financer.API/"]
COPY ["Financer.DataAccess/Financer.DataAccess.csproj", "Financer.DataAccess/"]
COPY ["Financer.Infrastructure/Financer.Infrastructure.csproj", "Financer.Infrastructure/"]
COPY ["Shared/SharedResources/SharedResources.csproj", "Shared/SharedResources/"]
RUN dotnet restore "Financer.API/Financer.API.csproj"

COPY Financer.API /Financer.API
COPY Financer.DataAccess /Financer.DataAccess
COPY Financer.Infrastructure /Financer.Infrastructure
COPY Shared/SharedResources /Shared/SharedResources

WORKDIR "/Financer.API"
RUN dotnet build "Financer.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Financer.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["./Financer.API"]
