# Развертывание проекта в Docker

Этот проект поддерживает два режима развертывания: Production и Development.

## Структура файлов

- `Dockerfile` - файл для сборки Docker образа
- `docker-compose.yml` - конфигурация для Production окружения
- `docker-compose.dev.yml` - конфигурация для Development окружения
- `.env` - переменные окружения для Production (создайте на основе .env.example)
- `.env.dev` - переменные окружения для Development
- `.dockerignore` - исключение файлов из контекста сборки

## Production окружение

### Особенности:
- Окружение: Production
- Порт: 80
- Контейнер: `directory-web-app`
- Использует внешнюю базу данных PostgreSQL

### Настройка:

1. Создайте файл `.env` на основе `.env.example`:
```bash
cp .env.example .env
```

2. Отредактируйте `.env` с параметрами вашей Production базы данных:
```env
POSTGRES_HOST=your-production-postgres-host
POSTGRES_PORT=5432
POSTGRES_DB=directory.webDb
POSTGRES_USER=postgres
POSTGRES_PASSWORD=your_password
ASPNETCORE_ENVIRONMENT=Production
```

### Запуск:

```bash
# Запуск в фоновом режиме
docker-compose up -d

# Запуск с пересборкой образа
docker-compose up --build -d

# Просмотр логов
docker-compose logs -f

# Остановка
docker-compose down
```

### Доступ:

Приложение доступно по адресу: **http://localhost:80**

## Development окружение

### Особенности:
- Окружение: Development
- Порт: 8383
- Контейнер: `directory-web-app-dev`
- Использует локальную базу данных PostgreSQL
- Поддерживает детализированное логирование и отладку

### Настройка:

Отредактируйте файл `.env.dev` с параметрами вашей локальной базы данных:
```env
POSTGRES_HOST=localhost
POSTGRES_PORT=5432
POSTGRES_DB=directory.webDb_dev
POSTGRES_USER=postgres
POSTGRES_PASSWORD=dev_password
ASPNETCORE_ENVIRONMENT=Development
```

### Запуск:

```bash
# Запуск в фоновом режиме
docker-compose -f docker-compose.dev.yml up -d

# Запуск с пересборкой образа
docker-compose -f docker-compose.dev.yml up --build -d

# Просмотр логов
docker-compose -f docker-compose.dev.yml logs -f

# Остановка
docker-compose -f docker-compose.dev.yml down
```

### Доступ:

Приложение доступно по адресу: **http://localhost:8383**

## Полезные команды

### Универсальные команды:

```bash
# Остановить все контейнеры
docker-compose down
docker-compose -f docker-compose.dev.yml down

# Просмотреть статус контейнеров
docker-compose ps
docker-compose -f docker-compose.dev.yml ps

# Пересобрать образ без кэша
docker-compose build --no-cache
docker-compose -f docker-compose.dev.yml build --no-cache

# Зайти внутрь контейнера
docker-compose exec web bash
docker-compose -f docker-compose.dev.yml exec web bash
```

### Для Development:

```bash
# Автоматический пересбор при изменении кода
docker-compose -f docker-compose.dev.yml watch
```

## Переключение между окружениями

### Из Production в Development:

```bash
# Остановить Production
docker-compose down

# Запустить Development
docker-compose -f docker-compose.dev.yml up -d
```

### Из Development в Production:

```bash
# Остановить Development
docker-compose -f docker-compose.dev.yml down

# Запустить Production
docker-compose up -d
```

## Одновременная работа

Вы можете запустить оба окружения одновременно, так как они используют:
- Разные порты (80 и 8383)
- Разные контейнеры
- Разные сети
- Разные базы данных (необязательно)

## Устранение проблем

### Ошибка "Connection refused":

Убедитесь, что база данных доступна и параметры подключения в `.env` или `.env.dev` верны.

### Порт уже занят:

Проверьте, какой контейнер использует порт:
```bash
# Windows
netstat -ano | findstr :80
netstat -ano | findstr :8383

# Linux/Mac
lsof -i :80
lsof -i :8383
```

### Очистка Docker:

```bash
# Удалить все контейнеры
docker-compose down -v
docker-compose -f docker-compose.dev.yml down -v

# Удалить образы
docker rmi $(docker images -q directoryweb-web)
```

## Структура приложения

Внутри контейнера приложение работает на порту 8080, но пробрасывается на разные порты хоста:
- Production: 80:8080
- Development: 8383:8080

## Мониторинг

```bash
# Просмотр ресурсов контейнеров
docker stats

# Просмотр событий Docker
docker events
```

## Резервное копирование

```bash
# Экспорт контейнера
docker export directory-web-app > app-backup.tar

# Импорт контейнера
docker import app-backup.tar directoryweb-web:backup