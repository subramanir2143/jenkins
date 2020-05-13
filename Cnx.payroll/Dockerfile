FROM mcr.microsoft.com/dotnet/core/sdk as build-image

WORKDIR /home/app

COPY ./Cnx.payroll/src/Services/EarningsAndDeductions ./

RUN dotnet test --verbosity=normal --results-directory /TestResults/ --logger "trx;LogFileName=test_results.xml" ./Cnx.EarningsAndDeductions.UnitTests/Cnx.EarningsAndDeductions.UnitTests.csproj

RUN dotnet publish ./Cnx.EarningsAndDeductions.API/Cnx.EarningsAndDeductions.API.csproj -o /publish/

FROM mcr.microsoft.com/dotnet/core/aspnet

WORKDIR /publish

COPY --from=build-image /publish .

COPY --from=build-image /TestResults /TestResults

ENTRYPOINT ["dotnet", "Cnx.EarningsAndDeductions.API.dll"]