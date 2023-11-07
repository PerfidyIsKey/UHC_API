# Learn about building .NET container images:
# https://github.com/dotnet/dotnet-docker/blob/main/samples/README.md
FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:7.0-jammy AS build
ARG TARGETARCH
WORKDIR /source
EXPOSE 80
# copy csproj and restore as distinct layers
COPY *.csproj .
RUN dotnet restore -a $TARGETARCH

# copy everything else and build app
COPY . .
RUN dotnet publish -a $TARGETARCH -o /app


# final stage/image
FROM mcr.microsoft.com/dotnet/aspnet:7.0-jammy
WORKDIR /app
EXPOSE 80
COPY --from=build /app .
USER $APP_UID
ENTRYPOINT ["./UHC_API"]