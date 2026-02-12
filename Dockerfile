# 1. Usar la imagen del SDK para compilar
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar el archivo de proyecto y restaurar dependencias
COPY ["MInimarketDaniela-Backend/MInimarketDaniela-Backend.csproj", "MInimarketDaniela-Backend/"]
RUN dotnet restore "MInimarketDaniela-Backend.csproj"

# Copiar todo el resto del código
COPY . .
WORKDIR "/src/."
RUN dotnet build "MInimarketDaniela-Backend.csproj" -c Release -o /app/build

# Publicar la aplicación
FROM build AS publish
RUN dotnet publish "MInimarketDaniela-Backend.csproj" -c Release -o /app/publish /p:UseAppHost=false

# 2. Usar la imagen de ejecución (más ligera) para correr la app
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MInimarketDaniela-Backend.dll"]
