FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY GestionOrdinateurs.Shared/GestionOrdinateurs.Shared.csproj GestionOrdinateurs.Shared/
COPY AuthentificationApi/AuthentificationApi.csproj AuthentificationApi/
RUN dotnet restore AuthentificationApi/AuthentificationApi.csproj

COPY GestionOrdinateurs.Shared/ GestionOrdinateurs.Shared/
COPY AuthentificationApi/ AuthentificationApi/
RUN dotnet publish AuthentificationApi/AuthentificationApi.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app
COPY --from=build /app/publish .
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080
ENTRYPOINT ["dotnet", "AuthentificationApi.dll"]