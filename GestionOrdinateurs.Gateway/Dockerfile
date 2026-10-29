FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY GestionOrdinateurs.Gateway/GestionOrdinateurs.Gateway.csproj GestionOrdinateurs.Gateway/
RUN dotnet restore GestionOrdinateurs.Gateway/GestionOrdinateurs.Gateway.csproj

COPY GestionOrdinateurs.Gateway/ GestionOrdinateurs.Gateway/
RUN dotnet publish GestionOrdinateurs.Gateway/GestionOrdinateurs.Gateway.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app
COPY --from=build /app/publish .
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080
ENTRYPOINT ["dotnet", "GestionOrdinateurs.Gateway.dll"]