FROM mcr.microsoft.com/dotnet/core/runtime:2.2-stretch-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["src/Services/EarningsAndDeductions/Cnx.EarningsAndDeductions.UnitTests/Cnx.EarningsAndDeductions.UnitTests.csproj", "src/Services/EarningsAndDeductions/Cnx.EarningsAndDeductions.UnitTests/"]
COPY ["src/Services/EarningsAndDeductions/Cnx.EarningsAndDeductions.API/Cnx.EarningsAndDeductions.API.csproj", "src/Services/EarningsAndDeductions/Cnx.EarningsAndDeductions.API/"]
COPY ["src/Services/EarningsAndDeductions/Cnx.EarningsAndDeductions.Domain/Cnx.EarningsAndDeductions.Domain.csproj", "src/Services/EarningsAndDeductions/Cnx.EarningsAndDeductions.Domain/"]
COPY ["src/Services/EarningsAndDeductions/Cnx.EarningsAndDeductions.Infrastructure/Cnx.EarningsAndDeductions.Infrastructure.csproj", "src/Services/EarningsAndDeductions/Cnx.EarningsAndDeductions.Infrastructure/"]
RUN dotnet restore "src/Services/EarningsAndDeductions/Cnx.EarningsAndDeductions.UnitTests/Cnx.EarningsAndDeductions.UnitTests.csproj"
COPY . .
WORKDIR "/src/src/Services/EarningsAndDeductions/Cnx.EarningsAndDeductions.UnitTests"
RUN dotnet test --verbosity=normal --results-directory /TestResults/ --logger "trx;LogFileName=test_results.xml"

FROM build AS publish
RUN dotnet publish "Cnx.EarningsAndDeductions.UnitTests.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
COPY --from=build /TestResults /TestResults
ENTRYPOINT ["dotnet", "Cnx.EarningsAndDeductions.UnitTests.dll"]