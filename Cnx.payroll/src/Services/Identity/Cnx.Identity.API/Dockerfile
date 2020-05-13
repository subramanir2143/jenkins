FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY ["src/Services/Identity/Cnx.Identity.API/Cnx.Identity.API.csproj", "src/Services/Identity/Cnx.Identity.API/"]
COPY ["src/BuildingBlocks/WebHostCustomization/WebHost.Customization/WebHost.Customization.csproj", "src/BuildingBlocks/WebHostCustomization/WebHost.Customization/"]
RUN dotnet restore "src/Services/Identity/Cnx.Identity.API/Cnx.Identity.API.csproj"
COPY . .
WORKDIR "/src/src/Services/Identity/Cnx.Identity.API"
RUN dotnet build "Cnx.Identity.API.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Cnx.Identity.API.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Cnx.Identity.API.dll"]