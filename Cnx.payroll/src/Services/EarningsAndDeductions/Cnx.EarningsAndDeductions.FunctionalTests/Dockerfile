FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build

WORKDIR /src

COPY ["src/Services/EarningsAndDeductions/Cnx.EarningsAndDeductions.FunctionalTests/Cnx.EarningsAndDeductions.FunctionalTests.csproj", "src/Services/EarningsAndDeductions/Cnx.EarningsAndDeductions.FunctionalTests/"]
COPY ["src/Services/EarningsAndDeductions/Cnx.EarningsAndDeductions.API/Cnx.EarningsAndDeductions.API.csproj", "src/Services/EarningsAndDeductions/Cnx.EarningsAndDeductions.API/"]
COPY ["src/Services/EarningsAndDeductions/Cnx.EarningsAndDeductions.Domain/Cnx.EarningsAndDeductions.Domain.csproj", "src/Services/EarningsAndDeductions/Cnx.EarningsAndDeductions.Domain/"]
COPY ["src/Services/EarningsAndDeductions/Cnx.EarningsAndDeductions.Infrastructure/Cnx.EarningsAndDeductions.Infrastructure.csproj", "src/Services/EarningsAndDeductions/Cnx.EarningsAndDeductions.Infrastructure/"]
RUN dotnet restore "src/Services/EarningsAndDeductions/Cnx.EarningsAndDeductions.FunctionalTests/Cnx.EarningsAndDeductions.FunctionalTests.csproj"
COPY . .

WORKDIR "/src/src/Services/EarningsAndDeductions/Cnx.EarningsAndDeductions.FunctionalTests/"

ENTRYPOINT ["dotnet", "test", "--verbosity=normal"]