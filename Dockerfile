# Estágio de Compilação
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /app

# Copiar arquivos csproj e restaurar dependências
COPY *.csproj ./
RUN dotnet restore

# Copiar os arquivos restantes e compilar
COPY . ./
RUN dotnet publish -c Release -o out

# Estágio de Execução
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build-env /app/out .

# Configurar a porta padrão do container (.NET 8 usa 8080 por padrão)
ENV ASPNETCORE_URLS=http://*:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "MeuErp.dll"]
