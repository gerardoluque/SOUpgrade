FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

COPY SOUpgrade.slnx .
COPY src/ src/
COPY tests/ tests/

RUN dotnet restore

RUN dotnet publish src/SOUpgrade.API/SOUpgrade.API.csproj -c Release -o /out

FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build /out .

EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

ENTRYPOINT ["dotnet", "SOUpgrade.API.dll"]
