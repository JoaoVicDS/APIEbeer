# ============================
# 1️⃣ Etapa de build
# ============================
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copia o arquivo da solução e restaura dependências
COPY ["APIEbeer.sln", "./"]
COPY ["APIEbeer.App/APIEbeer.App.csproj", "APIEbeer.App/"]
COPY ["APIEbeer.Controllers/APIEbeer.Controllers.csproj", "APIEbeer.Controllers/"]
COPY ["APIEbeer.Data/APIEbeer.Data.csproj", "APIEbeer.Data/"]
COPY ["APIEbeer.Services/APIEbeer.Services.csproj", "APIEbeer.Services/"]
COPY ["APIEbeer.Shared/APIEbeer.Shared.csproj", "APIEbeer.Shared/"]

# Restaura dependências
RUN dotnet restore "APIEbeer.sln"

# Copia o restante do código
COPY . .

# Compila em modo Release
RUN dotnet publish "APIEbeer.App/APIEbeer.App.csproj" -c Release -o /app/publish

# ============================
# 2️⃣ Etapa de runtime
# ============================
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Copia os arquivos publicados
COPY --from=build /app/publish .

# Expõe as portas HTTP e HTTPS
EXPOSE 5041

ENV ASPNETCORE_URLS="http://0.0.0.0:5041"

# Inicia a aplicação
ENTRYPOINT ["dotnet", "APIEbeer.App.dll"]