FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY AuthentificationKeycloakApi/AuthentificationKeycloakApi.csproj AuthentificationKeycloakApi/
RUN dotnet restore AuthentificationKeycloakApi/AuthentificationKeycloakApi.csproj

COPY AuthentificationKeycloakApi/ AuthentificationKeycloakApi/
RUN dotnet publish AuthentificationKeycloakApi/AuthentificationKeycloakApi.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app
COPY --from=build /app/publish .
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080
ENTRYPOINT ["dotnet", "AuthentificationKeycloakApi.dll"]