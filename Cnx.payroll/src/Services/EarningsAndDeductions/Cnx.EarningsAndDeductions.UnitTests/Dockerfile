FROM mcr.microsoft.com/dotnet/core/aspnet:2.2 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:2.2 AS build
WORKDIR /src
COPY ["src/Services/EarningsAndDeductions/Cnx.EarningsAndDeductions.UnitTests/Cnx.EarningsAndDeductions.UnitTests.csproj", "src/Services/EarningsAndDeductions/Cnx.EarningsAndDeductions.UnitTests/"]
COPY ["src/Services/EarningsAndDeductions/Cnx.EarningsAndDeductions.API/Cnx.EarningsAndDeductions.API.csproj", "src/Services/EarningsAndDeductions/Cnx.EarningsAndDeductions.API/"]
COPY ["src/Services/EarningsAndDeductions/Cnx.EarningsAndDeductions.Domain/Cnx.EarningsAndDeductions.Domain.csproj", "src/Services/EarningsAndDeductions/Cnx.EarningsAndDeductions.Domain/"]
COPY ["src/Services/EarningsAndDeductions/Cnx.EarningsAndDeductions.Infrastructure/Cnx.EarningsAndDeductions.Infrastructure.csproj", "src/Services/EarningsAndDeductions/Cnx.EarningsAndDeductions.Infrastructure/"]
RUN dotnet restore "src/Services/EarningsAndDeductions/Cnx.EarningsAndDeductions.UnitTests/Cnx.EarningsAndDeductions.UnitTests.csproj"
COPY . .

RUN dotnet test --verbosity=normal --results-directory /TestResults/ --logger "trx;LogFileName=test_results.xml" "src/Services/EarningsAndDeductions/Cnx.EarningsAndDeductions.UnitTests/Cnx.EarningsAndDeductions.UnitTests.csproj"

RUN dotnet publish "src/Services/EarningsAndDeductions/Cnx.EarningsAndDeductions.API/Cnx.EarningsAndDeductions.API.csproj" -o /publish/

FROM base AS final
WORKDIR /app
COPY --from=build /publish .
COPY --from=build /TestResults /TestResults
ENTRYPOINT ["dotnet", "Cnx.EarningsAndDeductions.API.dll"]