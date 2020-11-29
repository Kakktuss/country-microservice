FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src

COPY CountryApplication CountryApplication
COPY CountryApi CountryApi

ARG ACCESS_TOKEN
ARG ARTIFACTS_ENDPOINT

RUN wget -qO- https://raw.githubusercontent.com/Microsoft/artifacts-credprovider/master/helpers/installcredprovider.sh | bash

ENV NUGET_CREDENTIALPROVIDER_SESSIONTOKENCACHE_ENABLED true
ENV VSS_NUGET_EXTERNAL_FEED_ENDPOINTS "{\"endpointCredentials\": [{\"endpoint\":\"${ARTIFACTS_ENDPOINT}\", \"password\":\"${ACCESS_TOKEN}\"}]}"

RUN dotnet restore -s ${ARTIFACTS_ENDPOINT} -s "https://api.nuget.org/v3/index.json" "CountryApi/CountryApi.csproj"

FROM build AS publish
RUN ls
RUN dotnet publish ./CountryApi/CountryApi.csproj --no-restore -c Release -o /app

FROM base as final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "CountryApi.dll"]
