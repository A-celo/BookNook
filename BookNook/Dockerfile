# Usar la imagen oficial de .NET SDK 8.0
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["BookNook/BookNook.csproj", "BookNook/"]
RUN dotnet restore "BookNook/BookNook.csproj"
COPY . .
WORKDIR "/src/BookNook"
RUN dotnet build "BookNook.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BookNook.csproj" -c Release -o /app/publish

# Usar la imagen de ASP.NET para producción
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BookNook.dll"]

