FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["src/Services/Notification/Cnx.Notification.API/Cnx.Notification.API.csproj", "src/Services/Notification/Cnx.Notification.API/"]
RUN dotnet restore "src/Services/Notification/Cnx.Notification.API/Cnx.Notification.API.csproj"
COPY . .
WORKDIR "/src/src/Services/Notification/Cnx.Notification.API"
RUN dotnet build "Cnx.Notification.API.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Cnx.Notification.API.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Cnx.Notification.API.dll"]