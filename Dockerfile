FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /src

ARG nuget_pat

COPY CountryApplication CountryApplication
COPY CountryApi CountryApi

RUN wget -qO- https://raw.githubusercontent.com/Microsoft/artifacts-credprovider/master/helpers/installcredprovider.sh | bash

ENV NUGET_CREDENTIALPROVIDER_SESSIONTOKENCACHE_ENABLED true
ENV VSS_NUGET_EXTERNAL_FEED_ENDPOINTS '{"endpointCredentials":[{"endpoint":"https://pkgs.dev.azure.com/EcomLabLLC/_packaging/EcomLabLLC/nuget/v3/index.json","password":"'${nuget_pat}'"}]}'

RUN dotnet restore -s "https://pkgs.dev.azure.com/EcomLabLLC/_packaging/EcomLabLLC/nuget/v3/index.json" -s "https://api.nuget.org/v3/index.json" "CountryApi/CountryApi.csproj"

FROM build AS publish
RUN ls
RUN dotnet publish ./CountryApi/CountryApi.csproj --no-restore -c Release -o /app

FROM base as final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "CountryApi.dll"]
