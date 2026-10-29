FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY GestionOrdinateurs.Shared/GestionOrdinateurs.Shared.csproj GestionOrdinateurs.Shared/
COPY GestionOrdinateurs.Client/GestionOrdinateurs.Client.csproj GestionOrdinateurs.Client/
RUN dotnet restore GestionOrdinateurs.Client/GestionOrdinateurs.Client.csproj

COPY GestionOrdinateurs.Shared/ GestionOrdinateurs.Shared/
COPY GestionOrdinateurs.Client/ GestionOrdinateurs.Client/
RUN dotnet publish GestionOrdinateurs.Client/GestionOrdinateurs.Client.csproj -c Release -o /app/publish

FROM nginx:alpine
COPY --from=build /app/publish/wwwroot/ /usr/share/nginx/html/
COPY GestionOrdinateurs.Client/nginx.conf /etc/nginx/conf.d/default.conf

EXPOSE 5000