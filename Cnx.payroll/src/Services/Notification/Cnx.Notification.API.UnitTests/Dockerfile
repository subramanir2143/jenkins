FROM mcr.microsoft.com/dotnet/core/runtime:2.2-stretch-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["src/Services/Notification/Cnx.Notification.API.UnitTests/Cnx.Notification.API.UnitTests.csproj", "src/Services/Notification/Cnx.Notification.API.UnitTests/"]
COPY ["src/Services/Notification/Cnx.Notification.API/Cnx.Notification.API.csproj", "src/Services/Notification/Cnx.Notification.API/"]
RUN dotnet restore "src/Services/Notification/Cnx.Notification.API.UnitTests/Cnx.Notification.API.UnitTests.csproj"
COPY . .

RUN dotnet test --verbosity=normal --results-directory /TestResults/ --logger "trx;LogFileName=test_results.xml" "src/Services/Notification/Cnx.Notification.API.UnitTests/Cnx.Notification.API.csproj"

RUN dotnet publish "src/Services/Notification/Cnx.Notification.API/Cnx.Notification.API.csproj" -o /publish/

FROM base AS final
WORKDIR /app
COPY --from=build /publish .
COPY --from=build /TestResults /TestResults
ENTRYPOINT ["dotnet", "Cnx.Notification.API.dll"]