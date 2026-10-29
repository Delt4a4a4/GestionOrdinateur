FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY GestionOrdinateurs.Shared/GestionOrdinateurs.Shared.csproj GestionOrdinateurs.Shared/
COPY GestionOrdinateurs.API/GestionOrdinateurs.API.csproj GestionOrdinateurs.API/
RUN dotnet restore GestionOrdinateurs.API/GestionOrdinateurs.API.csproj

COPY GestionOrdinateurs.Shared/ GestionOrdinateurs.Shared/
COPY GestionOrdinateurs.API/ GestionOrdinateurs.API/
RUN dotnet publish GestionOrdinateurs.API/GestionOrdinateurs.API.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app
COPY --from=build /app/publish .
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080
ENTRYPOINT ["dotnet", "GestionOrdinateurs.API.dll"]