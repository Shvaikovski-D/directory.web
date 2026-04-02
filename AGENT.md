# AI Agent Instructions for directory.web

Этот файл содержит инструкции и руководство для AI агентов (Cline, Copilot и др.) по работе с проектом **directory.web**.

---

## 📋 Обзор проекта

**directory.web** - это веб-приложение на платформе **.NET** с архитектурой Clean Architecture, созданное на основе шаблона [Clean.Architecture.Solution.Template](https://github.com/jasontaylordev/CleanArchitecture) версии 10.8.0.

### Основные характеристики:
- **Архитектура**: Clean Architecture (Domain-Driven Design)
- **Технологический стек**: .NET 9+, ASP.NET Core 9.0+
- **API стиль**: Minimal API (без контроллеров)
- **ORM**: Entity Framework Core
- **CQRS**: MediatR для разделения команд и запросов
- **Документация**: Scalar API (доступно по корневому пути `/`)
- **Оркестрация**: .NET Aspire
- **Аутентификация**: ASP.NET Core Identity
- **Тип приложения**: Todo List management API

---

## 📁 Структура файлов и папок

```
directory.web/
├── src/
│   ├── Domain/                      # Доменный слой (бизнес-сущности и логика)
│   │   ├── Common/
│   │   │   ├── BaseAuditableEntity.cs    # Базовый класс с аудитом
│   │   │   ├── BaseEntity.cs             # Базовая сущность
│   │   │   ├── BaseEvent.cs              # Базовый класс событий
│   │   │   └── ValueObject.cs            # Базовый класс ValueObjects
│   │   ├── Entities/                      # Сущности домена
│   │   │   ├── TodoList.cs
│   │   │   └── TodoItem.cs
│   │   ├── ValueObjects/                  # Value Objects
│   │   │   └── Colour.cs
│   │   ├── Events/                        # Доменные события
│   │   │   └── TodoItemCompletedEvent.cs
│   │   ├── Enums/                         # Перечисления
│   │   │   └── PriorityLevel.cs
│   │   └── Exceptions/                    # Доменные исключения
│   │       └── UnsupportedColourException.cs
│   │
│   ├── Application/                 # Слой приложения (CQRS, бизнес-логика)
│   │   ├── Common/
│   │   │   ├── Behaviours/               # Pipeline behaviours
│   │   │   │   ├── AuthorizationBehaviour.cs
│   │   │   │   ├── ValidationBehaviour.cs
│   │   │   │   ├── LoggingBehaviour.cs
│   │   │   │   ├── PerformanceBehaviour.cs
│   │   │   │   └── UnhandledExceptionBehaviour.cs
│   │   │   ├── Interfaces/               # Интерфейсы сервиса
│   │   │   │   ├── IApplicationDbContext.cs
│   │   │   │   ├── IIdentityService.cs
│   │   │   │   └── IUser.cs
│   │   │   ├── Exceptions/
│   │   │   │   ├── ValidationException.cs
│   │   │   │   └── ForbiddenAccessException.cs
│   │   │   ├── Models/
│   │   │   │   ├── Result.cs
│   │   │   │   └── LookupDto.cs
│   │   │   └── Security/
│   │   │       └── AuthorizeAttribute.cs
│   │   │
│   │   ├── TodoLists/                     # Feature: Todo Lists
│   │   │   ├── Commands/
│   │   │   │   ├── CreateTodoList/
│   │   │   │   │   ├── CreateTodoList.cs           # Command + Handler
│   │   │   │   │   └── CreateTodoListCommandValidator.cs
│   │   │   │   ├── UpdateTodoList/
│   │   │   │   ├── DeleteTodoList/
│   │   │   │   └── UpdateTodoListDetail/
│   │   │   └── Queries/
│   │   │       └── GetTodos/
│   │   │           ├── GetTodos.cs                # Query + Handler
│   │   │           ├── TodoListDto.cs
│   │   │           ├── TodoItemDto.cs
│   │   │           └── TodosVm.cs
│   │   │
│   │   ├── TodoItems/                      # Feature: Todo Items
│   │   │   ├── Commands/
│   │   │   ├── EventHandlers/
│   │   │   └── Queries/
│   │   │
│   │   ├── WeatherForecasts/               # Feature: Weather (пример)
│   │   │   └── Queries/GetWeatherForecasts/
│   │   │
│   │   ├── GlobalUsings.cs
│   │   ├── DependencyInjection.cs         # DI настройки
│   │   └── Application.csproj
│   │
│   ├── Infrastructure/              # Инфраструктурный слой
│   │   ├── Data/
│   │   │   ├── ApplicationDbContext.cs      # EF Context
│   │   │   ├── ApplicationDbContextInitialiser.cs
│   │   │   ├── Configurations/              # EF конфигурации
│   │   │   │   ├── TodoListConfiguration.cs
│   │   │   │   └── TodoItemConfiguration.cs
│   │   │   ├── Interceptors/
│   │   │   │   ├── AuditableEntityInterceptor.cs
│   │   │   │   └── DispatchDomainEventsInterceptor.cs
│   │   │   └── Migrations/                  # EF миграции
│   │   │
│   │   ├── Identity/
│   │   │   ├── ApplicationUser.cs
│   │   │   ├── IdentityService.cs
│   │   │   └── IdentityResultExtensions.cs
│   │   │
│   │   ├── GlobalUsings.cs
│   │   ├── DependencyInjection.cs         # DI настройки
│   │   └── Infrastructure.csproj
│   │
│   ├── Web/                         # API слой (Minimal API)
│   │   ├── Endpoints/                      # API endpoints
│   │   │   ├── TodoLists.cs                # Реализует IEndpointGroup
│   │   │   ├── TodoItems.cs
│   │   │   ├── Users.cs
│   │   │   └── WeatherForecasts.cs
│   │   ├── Infrastructure/
│   │   │   ├── EndpointRouteBuilderExtensions.cs
│   │   │   ├── WebApplicationExtensions.cs
│   │   │   ├── IEndpointGroup.cs           # Интерфейс для групп эндпоинтов
│   │   │   ├── MethodInfoExtensions.cs
│   │   │   ├── ApiExceptionOperationTransformer.cs
│   │   │   ├── IdentityApiOperationTransformer.cs
│   │   │   ├── BearerSecuritySchemeTransformer.cs
│   │   │   └── ProblemDetailsExceptionHandler.cs
│   │   ├── Services/
│   │   │   └── CurrentUser.cs
│   │   ├── Program.cs                      # Точка входа
│   │   ├── DependencyInjection.cs
│   │   ├── GlobalUsings.cs
│   │   ├── Web.csproj
│   │   └── wwwroot/
│   │
│   ├── AppHost/                     # .NET Aspire оркестрация
│   │   ├── Program.cs
│   │   ├── Extensions.cs
│   │   ├── appsettings.json
│   │   └── AppHost.csproj
│   │
│   ├── ServiceDefaults/              # Общие сервисы Aspire
│   │   └── Extensions.cs
│   │
│   └── Shared/                      # Общие компоненты
│       └── Services.cs
│
├── tests/                           # Тесты
│   ├── Application.UnitTests/        # Юнит-тесты Application слоя
│   ├── Domain.UnitTests/            # Юнит-тесты Domain слоя
│   ├── Infrastructure.IntegrationTests/  # Интеграционные тесты
│   ├── Application.FunctionalTests/     # Функциональные тесты API
│   │   └── Infrastructure/
│   │       ├── TestBase.cs
│   │       ├── TestApp.cs
│   │       ├── WebApiFactory.cs
│   │       ├── FunctionalTestSetup.cs
│   │       └── DatabaseResetter.cs
│   └── TestAppHost/                 # Test Aspire host
│
├── README.md
├── AGENT.md                         # Этот файл
├── .editorconfig                    # Правила форматирования кода
├── .gitignore
├── directory.web.slnx              # Solution файл
├── global.json                      # .NET SDK версии
└── dotnet-tools.json                # Глобальные инструменты
```

---

## 🏗️ Архитектурные паттерны

### Clean Architecture Layers

1. **Domain Layer** (Ядро)
   - Сущности бизнес-логики
   - Value Objects
   - Domain Events
   - Бизнес-правила
   - **Нет зависимостей** от других слоев

2. **Application Layer**
   - Use Cases (Commands/Queries)
   - Handler-ы для CQRS
   - Валидация (FluentValidation)
   - Pipeline Behaviours
   - DTO для маппинга

3. **Infrastructure Layer**
   - Entity Framework DbContext
   - Миграции БД
   - Identity Service
   - Внешние сервисы

4. **Web/Presentation Layer**
   - Minimal API Endpoints
   - HTTP обработчики
   - Аутентификация/авторизация

### CQRS Pattern

**Команды (Commands)** - для изменения данных:
```csharp
public record CreateTodoListCommand : IRequest<int>
{
    public string? Title { get; init; }
    public string? Colour { get; init; }
}

public class CreateTodoListCommandHandler : IRequestHandler<CreateTodoListCommand, int>
{
    private readonly IApplicationDbContext _context;
    
    public CreateTodoListCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<int> Handle(CreateTodoListCommand request, CancellationToken cancellationToken)
    {
        var entity = new TodoList { ... };
        _context.TodoLists.Add(entity);
        await _context.SaveChangesAsync(cancellationToken);
        return entity.Id;
    }
}
```

**Запросы (Queries)** - для чтения данных:
```csharp
public record GetTodosQuery : IRequest<TodosVm>;

public class GetTodosQueryHandler : IRequestHandler<GetTodosQuery, TodosVm>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    
    public async Task<TodosVm> Handle(GetTodosQuery request, CancellationToken cancellationToken)
    {
        var lists = await _context.TodoLists
            .Include(x => x.Items)
            .ToListAsync(cancellationToken);
        
        return new TodosVm { Lists = _mapper.Map<List<TodoListDto>>(lists) };
    }
}
```

### Pipeline Behaviours

Behaviours обрабатывают запросы в порядке:
1. **Unsealed (Performance)**
2. **Authorization**
3. **Validation**
4. **Logging**
5. **Unhandled Exception**

---

## 📝 Стиль кодирования

### Правила форматирования (из .editorconfig)

**Отступы и пробелы:**
- C# файлы: 4 пробела на уровень
- XML/JSON/JS файлы: 2 пробела
- Конец строки: LF (Unix style)
- Финальная новая строка: обязательна

**Стиль кода:**
- Использовать `var` ТОЛЬКО когда тип очевиден из контекста
- Предпочитать встроенные типы (`int`, `string`) вместо `Int32`, `String`
- Использовать null-propagation (`?.`)
- Использовать expression-bodied члены для свойств и аксессоров
- Поля должны быть `readonly` где возможно
- Использовать primary constructors для record и простых классов

**Использование using:**
- Using directives должны быть **вне namespace**
- Сортировка: System -> Third party -> Application namespace

### Организация кода

```csharp
// 1. System namespaces
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// 2. Third-party namespaces
using MediatR;
using Microsoft.EntityFrameworkCore;

// 3. Application namespaces
using directory.web.Application.Common.Interfaces;
using directory.web.Domain.Entities;
using directory.web.Domain.ValueObjects;

namespace directory.web.Application.TodoLists.Commands.CreateTodoList;
```

---

## 🏷️ Соглашения по наименованию

### Naming Conventions

| Тип | Стиль | Пример | Примечание |
|-----|-------|--------|------------|
| **Классы** | PascalCase | `TodoList`, `CreateTodoListCommand` | Существительные или герунды |
| **Интерфейсы** | IPascalCase | `IApplicationDbContext`, `IUser` | С префиксом `I` |
| **Методы** | PascalCase | `GetAll`, `CreateUser`, `Validate` | Глагол или Verb + Noun |
| **Свойства** | PascalCase | `Title`, `CreatedAt`, `Items` | Существительные |
| **Параметры** | camelCase | `userId`, `pageNumber`, `cancellationToken` | Существительные |
| **Локальные переменные** | camelCase | `userList`, `totalItems` | Существительные |
| **Приватные поля** | _camelCase | `_context`, `_mapper` | С подчёркиванием |
| **Статические приватные поля** | s_camelCase | `s_logger`, `s_instance` | `s_` префикс |
| **Публичные константы** | PascalCase | `MaxItems`, `DefaultTimeout` | |
| **Перечисления** | PascalCase | `PriorityLevel`, `TodoStatus` | |
| **Type Parameters** | TTypeName | `TEntity`, `TRequest`, `TResponse` | С префиксом `T` |
| **DTO** | PascalCase | `TodoListDto`, `TodoItemDto`, `TodosVm` | С суффиксом `Dto` или `Vm` |
| **ViewModels** | PascalCase | `TodosVm` | С суффиксом `Vm` |

### Паттерны наименования файлов

**Commands:**
- Файл: `CreateTodoListCommand.cs`
- Содержит: Command record + Handler class

**Queries:**
- Файл: `GetTodosQuery.cs`
- Содержит: Query record + Handler class

**Validators:**
- Файл: `CreateTodoListCommandValidator.cs`
- Наследует: `AbstractValidator<CreateTodoListCommand>`

**Endpoints:**
- Файл: `TodoLists.cs`
- Класс: `TodoLists` (реализует `IEndpointGroup`)

**Entities:**
- Файл: `TodoList.cs`
- Наследует: `BaseAuditableEntity` или `BaseEntity`

**ValueObjects:**
- Файл: `Colour.cs`
- Наследует: `ValueObject`

### Пространства имён

Следует папочной структуре:
```csharp
namespace directory.web.Domain.Entities;
namespace directory.web.Application.TodoLists.Commands.CreateTodoList;
namespace directory.web.Web.Endpoints;
```

---

## ✅ Best Practices для генерации кода

### 1. Создание новой сущности (Entity)

**Шаг 1: Создайте доменную сущность**
```csharp
// src/Domain/Entities/Product.cs
namespace directory.web.Domain.Entities;

public class Product : BaseAuditableEntity
{
    public string Name { get; set; } = string.Empty;
    
    public string Description { get; set; } = string.Empty;
    
    public decimal Price { get; set; }
    
    public int CategoryId { get; set; }
    
    public Category Category { get; set; } = null!;
}
```

**Шаг 2: Добавьте в DbContext**
```csharp
// src/Infrastructure/Data/ApplicationDbContext.cs
public DbSet<Product> Products => Set<Product>();
```

**Шаг 3: Создайте EF конфигурацию**
```csharp
// src/Infrastructure/Data/Configurations/ProductConfiguration.cs
using directory.web.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace directory.web.Infrastructure.Data.Configurations;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.Property(t => t.Name)
            .HasMaxLength(200)
            .IsRequired();
        
        builder.Property(t => t.Price)
            .HasPrecision(18, 2);
        
        builder.HasOne(t => t.Category)
            .WithMany()
            .HasForeignKey(t => t.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
```

**Шаг 4: Создайте миграцию**
```bash
dotnet ef migrations add AddProductEntity --project src/Infrastructure --startup-project src/Web --context ApplicationDbContext --output-dir Data/Migrations
```

---

### 2. Создание новой команды (Command)

**Использование шаблона:**
```bash
cd src/Application
dotnet new ca-usecase --name CreateProduct --feature-name Products --usecase-type command --return-type int
```

**Ручное создание:**

```csharp
// src/Application/Products/Commands/CreateProduct/CreateProduct.cs
using directory.web.Application.Common.Interfaces;
using directory.web.Domain.Entities;

namespace directory.web.Application.Products.Commands.CreateProduct;

public record CreateProductCommand : IRequest<int>
{
    public string Name { get; init; } = string.Empty;
    
    public string Description { get; init; } = string.Empty;
    
    public decimal Price { get; init; }
    
    public int CategoryId { get; init; }
}

public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, int>
{
    private readonly IApplicationDbContext _context;

    public CreateProductCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var entity = new Product
        {
            Name = request.Name,
            Description = request.Description,
            Price = request.Price,
            CategoryId = request.CategoryId
        };

        _context.Products.Add(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
```

---

### 3. Создание валидатора (Validator)

```csharp
// src/Application/Products/Commands/CreateProduct/CreateProductCommandValidator.cs
using directory.web.Application.Common.Interfaces;

namespace directory.web.Application.Products.Commands.CreateProduct;

public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    private readonly IApplicationDbContext _context;

    public CreateProductCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(v => v.Name)
            .NotEmpty()
            .MaximumLength(200)
            .MustAsync(BeUniqueName)
                .WithMessage("'{PropertyName}' must be unique.")
                .WithErrorCode("Unique");
        
        RuleFor(v => v.Price)
            .GreaterThan(0);
        
        RuleFor(v => v.CategoryId)
            .GreaterThan(0)
            .MustAsync(CategoryExists)
                .WithMessage("Category does not exist.");
    }

    public async Task<bool> BeUniqueName(string name, CancellationToken cancellationToken)
    {
        return !await _context.Products
            .AnyAsync(p => p.Name == name, cancellationToken);
    }
    
    public async Task<bool> CategoryExists(int categoryId, CancellationToken cancellationToken)
    {
        return await _context.Categories
            .AnyAsync(c => c.Id == categoryId, cancellationToken);
    }
}
```

---

### 4. Создание запроса (Query)

```csharp
// src/Application/Products/Queries/GetProducts/GetProducts.cs
using directory.web.Application.Common.Interfaces;
using directory.web.Application.Common.Models;
using AutoMapper;
using MediatR;

namespace directory.web.Application.Products.Queries.GetProducts;

public record GetProductsQuery : IRequest<PaginatedList<ProductDto>>
{
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
    public string? SearchTerm { get; init; }
}

public class GetProductsQueryHandler : IRequestHandler<GetProductsQuery, PaginatedList<ProductDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetProductsQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<ProductDto>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Products
            .Include(p => p.Category)
            .AsQueryable();
        
        if (!string.IsNullOrEmpty(request.SearchTerm))
        {
            query = query.Where(p => 
                p.Name.Contains(request.SearchTerm) ||
                p.Description.Contains(request.SearchTerm));
        }
        
        return await query
            .OrderBy(p => p.Name)
            .ProjectTo<ProductDto>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(request.PageNumber, request.PageSize);
    }
}
```

---

### 5. Создание эндпоинта (Endpoint)

```csharp
// src/Web/Endpoints/Products.cs
using directory.web.Application.Products.Commands.CreateProduct;
using directory.web.Application.Products.Queries.GetProducts;
using Microsoft.AspNetCore.Http.HttpResults;

namespace directory.web.Web.Endpoints;

public class Products : IEndpointGroup
{
    public static void Map(RouteGroupBuilder groupBuilder)
    {
        groupBuilder.RequireAuthorization();

        groupBuilder.MapGet(GetProducts);
        groupBuilder.MapPost(CreateProduct);
        groupBuilder.MapGet(GetProduct, "{id}");
        groupBuilder.MapPut(UpdateProduct, "{id}");
        groupBuilder.MapDelete(DeleteProduct, "{id}");
    }

    [EndpointSummary("Get all products")]
    [EndpointDescription("Retrieves a paginated list of products with optional filtering.")]
    public static async Task<Ok<PaginatedList<ProductDto>>> GetProducts(
        ISender sender, 
        [AsParameters] GetProductsQuery query)
    {
        var result = await sender.Send(query);
        return TypedResults.Ok(result);
    }

    [EndpointSummary("Create a new product")]
    [EndpointDescription("Creates a new product and returns its ID.")]
    public static async Task<Created<int>> CreateProduct(
        ISender sender, 
        CreateProductCommand command)
    {
        var id = await sender.Send(command);
        return TypedResults.Created($"/{nameof(Products)}/{id}", id);
    }

    [EndpointSummary("Get a product by ID")]
    [EndpointDescription("Retrieves a single product by its unique identifier.")]
    public static async Task<Results<Ok<ProductDto>, NotFound>> GetProduct(
        ISender sender, 
        int id)
    {
        // Implement GetProductQuery and handler first
        return TypedResults.Ok(new ProductDto());
    }
}
```

---

### 6. Авторизация

**Добавьте атрибут авторизации:**
```csharp
[Authorize(Roles = Roles.Administrator)]
public class DeleteTodoListCommand : IRequest
{
    public int Id { get; init; }
}
```

**Или на уровне эндпоинта:**
```csharp
groupBuilder.MapGet(GetProducts);
groupBuilder.MapPost(CreateProduct)
    .RequireAuthorization(Roles.Administrator);
```

**Проверка прав в Behaviour:**
```csharp
public class AuthorizationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
{
    private readonly ICurrentUserService _userService;
    private readonly IIdentityService _identityService;

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var authorizeAttributes = request.GetType().GetCustomAttributes<AuthorizeAttribute>();

        if (authorizeAttributes.Any())
        {
            // Проверка аутентификации
            if (_userService.UserId == null)
            {
                throw new UnauthorizedAccessException();
            }

            // Проверка ролей
            var authorizeAttributesWithRoles = authorizeAttributes.Where(a => !string.IsNullOrWhiteSpace(a.Roles));
            if (authorizeAttributesWithRoles.Any())
            {
                foreach (var roles in authorizeAttributesWithRoles.Select(a => a.Roles.Split(',')))
                {
                    foreach (var role in roles)
                    {
                        var authorized = await _identityService.IsInRoleAsync(_userService.UserId, role.Trim());
                        if (!authorized) throw new ForbiddenAccessException();
                    }
                }
            }
        }

        return await next();
    }
}
```

---

### 7. Обработка ошибок

**Возврат правильных HTTP кодов:**
```csharp
public static async Task<Results<NoContent, NotFound, BadRequest>> UpdateTodoList(
    ISender sender, 
    int id, 
    UpdateTodoListCommand command)
{
    if (id != command.Id) return TypedResults.BadRequest();

    await sender.Send(command);

    return TypedResults.NoContent();
}
```

**Пользовательские исключения:**
```csharp
// src/Application/Common/Exceptions/ValidationException.cs
public class ValidationException : Exception
{
    public IDictionary<string, string[]> Errors { get; }

    public ValidationException(IDictionary<string, string[]> errors)
        : base("One or more validation failures have occurred.")
    {
        Errors = errors;
    }
}
```

---

### 8. Использование Domain Events

```csharp
// 1. Создайте доменное событие
// src/Domain/Events/ProductCreatedEvent.cs
namespace directory.web.Domain.Events;

public class ProductCreatedEvent : BaseEvent
{
    public int ProductId { get; }
    public string ProductName { get; }

    public ProductCreatedEvent(int productId, string productName)
    {
        ProductId = productId;
        ProductName = productName;
    }
}

// 2. Добавьте событие в сущность
// src/Domain/Entities/Product.cs
public class Product : BaseAuditableEntity
{
    // ... other properties
    
    private readonly List<BaseEvent> _domainEvents = new();
    public IReadOnlyCollection<BaseEvent> DomainEvents => _domainEvents.AsReadOnly();

    public static Product Create(string name, decimal price)
    {
        var product = new Product { Name = name, Price = price };
        product._domainEvents.Add(new ProductCreatedEvent(0, name));
        return product;
    }
}

// 3. Создайте обработчик события
// src/Application/Products/EventHandlers/ProductCreated.cs
using directory.web.Domain.Events;

namespace directory.web.Application.Products.EventHandlers;

public class ProductCreatedHandler : INotificationHandler<ProductCreatedEvent>
{
    private readonly ILogger<ProductCreatedHandler> _logger;

    public ProductCreatedHandler(ILogger<ProductCreatedHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(ProductCreatedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Product created: {ProductId} - {ProductName}", 
            notification.ProductId, notification.ProductName);
        
        return Task.CompletedTask;
    }
}
```

---

### 9. Value Objects

```csharp
// src/Domain/ValueObjects/Price.cs
using directory.web.Domain.Common;

namespace directory.web.Domain.ValueObjects;

public sealed class Price : ValueObject
{
    public decimal Value { get; }
    public string Currency { get; } = "USD";

    private Price(decimal value, string currency)
    {
        Value = value;
        Currency = currency;
    }

    public static Price From(decimal value, string currency = "USD")
    {
        if (value < 0)
        {
            throw new ValidationException("Price cannot be negative.");
        }

        return new Price(value, currency);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
        yield return Currency;
    }

    public override string ToString() => $"{Currency} {Value:F2}";
}

// Использование в сущности
public class Product : BaseAuditableEntity
{
    public Price Price { get; private set; } = Price.From(0);
    
    public void UpdatePrice(decimal newPrice)
    {
        Price = Price.From(newPrice);
    }
}
```

---

## 🚀 Команды для разработки

### Запуск проекта
```bash
# Запуск с AppHost
dotnet run --project src/AppHost

# Запуск только Web API
dotnet run --project src/Web
```

### Сборка
```bash
dotnet build
```

### Тесты
```bash
# Все тесты
dotnet test

# Конкретный проект тестов
dotnet test tests/Application.UnitTests
dotnet test tests/Domain.UnitTests
dotnet test tests/Infrastructure.IntegrationTests
dotnet test tests/Application.FunctionalTests
```

### EF Core миграции
```bash
# Создать миграцию
dotnet ef migrations add MigrationName --project src/Infrastructure --startup-project src/Web --context ApplicationDbContext --output-dir Data/Migrations

# Применить миграции
dotnet ef database update --project src/Infrastructure --startup-project src/Web --context ApplicationDbContext

# Откат последней миграции
dotnet ef database update --project src/Infrastructure --startup-project src/Web --context ApplicationDbContext 0

# Удалить последнюю миграцию
dotnet ef migrations remove --project src/Infrastructure --startup-project src/Web --context ApplicationDbContext
```

### Scaffold новых use cases
```bash
cd src/Application

# Создать команду
dotnet new ca-usecase --name CreateProduct --feature-name Products --usecase-type command --return-type int

# Создать запрос
dotnet new ca-usecase --name GetProducts --feature-name Products --usecase-type query --return-type PaginatedList<ProductDto>

# Если шаблон не найден, установите его:
dotnet new install Clean.Architecture.Solution.Template::10.8.0
```

### Установка инструментов
```bash
dotnet tool install --global dotnet-ef
```

---

## 📚 Важные файлы и их назначение

### Конфигурация
- **.editorconfig** - Правила форматирования и стиля кода
- **global.json** - Версия .NET SDK
- **dotnet-tools.json** - Глобальные инструменты .NET

### DI (Dependency Injection)
- **src/Application/DependencyInjection.cs** - Регистрация Application сервисов
- **src/Infrastructure/DependencyInjection.cs** - Регистрация Infrastructure сервисов
- **src/Web/DependencyInjection.cs** - Регистрация Web сервисов

### Точка входа
- **src/Web/Program.cs** - Основной файл конфигурации приложения

### Интерфейсы
- **IApplicationDbContext** - Интерфейс для работы с БД (моки для тестов)
- **IIdentityService** - Интерфейс для работы с Identity
- **IUser** - Текущий пользователь
- **IEndpointGroup** - Интерфейс для групп эндпоинтов

---

## ⚠️ Важные замечания

### Что нужно помнить:
1. **CORS настроен для всех источников** (разрешено для разработки, измените для production)
2. **HTTPS редирект отключен** (`app.UseHttpsRedirection()` закомментирован)
3. **Swagger отключен**, используется Scalar для документации API
4. **Все эндпоинты требуют авторизации** по умолчанию (добавьте `.AllowAnonymous()` если нужно)
5. **Аудит включён** автоматически (Created, CreatedBy, LastModified, LastModifiedBy)
6. **Domain Events** автоматически диспетчатятся через DispatchDomainEventsInterceptor

### Безопасность:
- Секреты храните в **Azure Key Vault** или **User Secrets**
- Не храните пароли или connection strings в коде
- Используйте `[Authorize]` для защищённых эндпоинтов
- Валидируйте все входные данные с помощью FluentValidation

### Производительность:
- Используйте `.AsNoTracking()` для read-only запросов
- Загружайте связанные сущности только при необходимости (`.Include()`)
- Используйте пагинацию для больших списков
- Кэшируйте данные которые редко меняются

### Тестирование:
- **Unit Tests** - тестируйте бизнес-логику изолированно
- **Integration Tests** - тестируйте интеграцию с БД
- **Functional Tests** - тестируйте API эндпоинты
- Всегда мокайте внешние зависимости

---

## 🎯 Чек-лист для добавления новой функции

При добавлении новой функциональности следуйте этому порядку:

1. [ ] **Domain**: Создайте сущности, Value Objects, Domain Events
2. [ ] **Infrastructure**: 
    - [ ] Добавьте DbSet в ApplicationDbContext
    - [ ] Создайте EF конфигурацию
    - [ ] Создайте миграцию
    - [ ] Примените миграцию
3. [ ] **Application**:
    - [ ] Создайте Command/Query с Handler
    - [ ] Создайте Validator
    - [ ] Создайте DTO/ViewModels
    - [ ] Добавьте обработчики Domain Events (если нужны)
4. [ ] **Web**:
    - [ ] Создайте файл эндпоинтов
    - [ ] Реализуйте IEndpointGroup
    - [ ] Добавьте маршруты с правильными HTTP методами
    - [ ] Добавьте атрибуты документации (EndpointSummary, EndpointDescription)
    - [ ] Возвратите правильные HTTP коды ответа
5. [ ] **Tests**:
    - [ ] Напишите unit tests для Application слоя
    - [ ] Напишите integration tests для Infrastructure
    - [ ] Напишите functional tests для API
6. [ ] **Документация**: Обновите этот файл если добавили новые паттерны

---

## 🔧 Решение распространённых проблем

### Проблема: "No templates or subcommands found matching: 'ca-usecase'"
**Решение:**
```bash
dotnet new install Clean.Architecture.Solution.Template::10.8.0
```

### Проблема: Миграции не применяются
**Решение:**
```bash
# Проверьте строку подключения
# Убедитесь что база данных существует
# Примените миграции явно:
dotnet ef database update --project src/Infrastructure --startup-project src/Web --context ApplicationDbContext
```

### Проблема: Валидация не работает
**Решение:**
Убедитесь что:
1. Класс валидатора наследуется от `AbstractValidator<T>`
2. Валидатор зарегистрирован в DI (через `AddValidatorsFromAssembly`)
3. ValidationBehaviour зарегистрирован в DI

### Проблема: Domain Events не выполняются
**Решение:**
Убедитесь что:
1. Событие добавлено в коллекцию `DomainEvents` в сущности
2. Handler реализует `INotificationHandler<TEvent>`
3. DispatchDomainEventsInterceptor зарегистрирован в DI

---

## 📖 Полезные ресурсы

- [Clean Architecture](https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html)
- [MediatR Documentation](https://github.com/jimmybogard/MediatR)
- [FluentValidation](https://fluentvalidation.net/)
- [Entity Framework Core](https://learn.microsoft.com/en-us/ef/core/)
- [ASP.NET Core Minimal APIs](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/minimal-apis)
- [Clean Architecture Template](https://github.com/jasontaylordev/CleanArchitecture)

---

**Последнее обновление:** 2026-04-02

**Для вопросов и предложений по улучшению этого файла, пожалуйста, создайте issue или PR.**