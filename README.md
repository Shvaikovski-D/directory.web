# directory.web

The project was generated using the [Clean.Architecture.Solution.Template](https://github.com/jasontaylordev/CleanArchitecture) version 10.8.0.

## Build

Run `dotnet build` to build the solution.

## Run

To run the application:

```bash
dotnet run --project .\src\AppHost
```

The Aspire dashboard will open automatically, showing the application URLs and logs.

## Code Styles & Formatting

The template includes [EditorConfig](https://editorconfig.org/) support to help maintain consistent coding styles for multiple developers working on the same project across various editors and IDEs. The **.editorconfig** file defines the coding styles applicable to this solution.

## Code Scaffolding

The template includes support to scaffold new commands and queries.

Start in the `.\src\Application\` folder.

Create a new command:

```
dotnet new ca-usecase --name CreateTodoList --feature-name TodoLists --usecase-type command --return-type int
```

Create a new query:

```
dotnet new ca-usecase -n GetTodos -fn TodoLists -ut query -rt TodosVm
```

If you encounter the error *"No templates or subcommands found matching: 'ca-usecase'."*, install the template and try again:

```bash
dotnet new install Clean.Architecture.Solution.Template::10.8.0
```

## Test

The solution contains unit, integration, and functional tests.

To run the tests:
```bash
dotnet test
```

## Help
To learn more about the template go to the [project website](https://cleanarchitecture.jasontaylor.dev). Here you can find additional guidance, request new features, report a bug, and discuss the template with other users.

## Docker Deployment

Этот проект поддерживает развертывание в Docker с двумя режимами: Production и Development.

### Production

- **Порт**: 80
- **Окружение**: Production
- **Доступ**: http://localhost:80

Запуск Production окружения:
```bash
docker-compose up -d
```

Подробнее в [DOCKER_DEPLOYMENT.md](./DOCKER_DEPLOYMENT.md#production-окружение)

### Development

- **Порт**: 8383
- **Окружение**: Development
- **Доступ**: http://localhost:8383

Запуск Development окружения:
```bash
docker-compose -f docker-compose.dev.yml up -d
```

Подробнее в [DOCKER_DEPLOYMENT.md](./DOCKER_DEPLOYMENT.md#development-окружение)

### Структура Docker файлов

- `Dockerfile` - файл для сборки Docker образа
- `docker-compose.yml` - конфигурация Production
- `docker-compose.dev.yml` - конфигурация Development
- `.env` - переменные окружения Production
- `.env.dev` - переменные окружения Development
- `.dockerignore` - исключение файлов из сборки

Полная документация по Docker развертыванию доступна в файле [DOCKER_DEPLOYMENT.md](./DOCKER_DEPLOYMENT.md).

## Migrations
```
dotnet ef migrations add InitialCreate --project src/Infrastructure --startup-project src/Web --context directory.web.Infrastructure.Data.ApplicationDbContext --output-dir Data/Migrations
dotnet ef database update --project src/Infrastructure --startup-project src/Web --context ApplicationDbContext
```
 ++++++++ REPLACE
