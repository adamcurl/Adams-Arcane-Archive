#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base

WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
ENV DOTNET_CLI_TELEMETRY_OPTOUT=true \
    DOTNET_SKIP_FIRST_TIME_EXPERIENCE=true
WORKDIR /src
COPY "AdamsArcaneArchive.csproj" .
RUN dotnet restore "AdamsArcaneArchive.csproj"
COPY . .
RUN dotnet build "AdamsArcaneArchive.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AdamsArcaneArchive.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AdamsArcaneArchive.dll"]
