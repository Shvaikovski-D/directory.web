# Этап сборки
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Копируем файлы проекта и решения
COPY ["Directory.Build.props", "./"]
COPY ["Directory.Packages.props", "./"]
COPY ["src/Web/Web.csproj", "Web/"]
COPY ["src/Application/Application.csproj", "Application/"]
COPY ["src/Domain/Domain.csproj", "Domain/"]
COPY ["src/Infrastructure/Infrastructure.csproj", "Infrastructure/"]
COPY ["src/ServiceDefaults/ServiceDefaults.csproj", "ServiceDefaults/"]
COPY ["src/Shared/Shared.csproj", "Shared/"]

# Восстанавливаем зависимости для всех проектов
RUN dotnet restore "Web/Web.csproj"

# Копируем исходный код проектов
COPY ["src/Web/", "Web/"]
COPY ["src/Application/", "Application/"]
COPY ["src/Domain/", "Domain/"]
COPY ["src/Infrastructure/", "Infrastructure/"]
COPY ["src/ServiceDefaults/", "ServiceDefaults/"]
COPY ["src/Shared/", "Shared/"]

# Собираем проект Web
WORKDIR "/src/Web"
RUN dotnet build "Web.csproj" -c Release -o /app/build

# Публикуем проект
FROM build AS publish
RUN dotnet publish "Web.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Этап выполнения
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
EXPOSE 80
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "directory.web.Web.dll"]