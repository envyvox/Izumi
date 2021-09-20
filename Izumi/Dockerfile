FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build-env

# Copy everything else and build
COPY Izumi ./Izumi
COPY Izumi.Data ./Izumi.Data
COPY Izumi.Services ./Izumi.Services

RUN dotnet restore Izumi/Izumi.csproj
RUN dotnet publish Izumi/*.csproj -c Release -o out

# Build runtime image
WORKDIR /
FROM mcr.microsoft.com/dotnet/aspnet:5.0
COPY --from=build-env ./out .
ENTRYPOINT ["dotnet", "Izumi.dll"]
