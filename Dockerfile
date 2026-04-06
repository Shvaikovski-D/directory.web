# Этап сборки
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Debug
WORKDIR /src

# Копируем файлы проекта и решения
COPY ["Directory.Build.props", "./"]
COPY ["Directory.Packages.props", "./"]

# Копируем .csproj файлы для кэширования зависимостей
COPY ["src/Web/Web.csproj", "Web/"]
COPY ["src/Application/Application.csproj", "Application/"]
COPY ["src/Domain/Domain.csproj", "Domain/"]
COPY ["src/Infrastructure/Infrastructure.csproj", "Infrastructure/"]
COPY ["src/ServiceDefaults/ServiceDefaults.csproj", "ServiceDefaults/"]
COPY ["src/Shared/Shared.csproj", "Shared/"]

# Восстанавливаем зависимости (отдельный слой для кэширования)
RUN dotnet restore "Web/Web.csproj"

# Копируем исходный код
COPY ["src/Web/", "Web/"]
COPY ["src/Application/", "Application/"]
COPY ["src/Domain/", "Domain/"]
COPY ["src/Infrastructure/", "Infrastructure/"]
COPY ["src/ServiceDefaults/", "ServiceDefaults/"]
COPY ["src/Shared/", "Shared/"]

# Публикуем проект
WORKDIR "/src/Web"
RUN dotnet publish "Web.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Этап publish для копирования файлов
FROM build AS publish

# Этап выполнения
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app

# Получаем порт из переменной окружения или используем 8080 по умолчанию для Render.com
ENV ASPNETCORE_URLS=http://+:8080

# Render.com устанавливает переменную PORT автоматически
# Приложение должно использовать этот порт
EXPOSE 8080

COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "directory.web.Web.dll"]
