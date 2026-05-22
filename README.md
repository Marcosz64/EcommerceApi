# ECommerce API

API REST para gestión de productos y órdenes de compra desarrollada en .NET 8 con arquitectura limpia (Clean Architecture), CQRS con MediatR, validación con FluentValidation, Entity Framework Core y SQLite.

## Stack tecnológico

- .NET 8
- ASP.NET Core Web API
- Entity Framework Core 8 + SQLite
- Swagger / OpenAPI
- MediatR (CQRS)
- FluentValidation (validación de comandos con pipeline behavior)
- Autenticación JWT Bearer
- BCrypt.Net-Next

## Arquitectura del proyecto

```
ECommerce.sln
├── ECommerce.Api          — API Web (controladores, Program.cs)
├── ECommerce.Application  — Casos de uso, DTOs, interfaces, comandos/consultas CQRS
├── ECommerce.Domain       — Entidades de negocio y reglas de dominio
└── ECommerce.Infrastructure — Persistencia (EF Core), repositorios, servicios externos
```

Los controladores usan **MediatR** para enviar comandos y consultas. Cada operación CRUD tiene su propio `IRequest` y `IRequestHandler` en la capa Application.

## Endpoints de la API

- **`POST /api/auth/register`** — Registrar un nuevo usuario con rol User (público)
- **`POST /api/auth/login`** — Iniciar sesión y obtener JWT (público)
- **`GET /api/products`** — Listar todos los productos (requiere autenticación)
- **`GET /api/products/{id}`** — Obtener un producto por ID (requiere autenticación)
- **`POST /api/products`** — Crear un producto (requiere rol Admin)
- **`PUT /api/products/{id}`** — Actualizar un producto existente (requiere rol Admin)
- **`DELETE /api/products/{id}`** — Eliminar un producto (requiere rol Admin)
- **`POST /api/orders`** — Crear una orden de compra con productos (requiere autenticación)

### Ejemplos de uso

**Registro** — `POST /api/auth/register`
```json
{
  "name": "Juan Pérez",
  "email": "juan@example.com",
  "password": "MiPassword123!"
}
```

**Login** — `POST /api/auth/login`
```json
{
  "email": "admin@test.com",
  "password": "Admin123!"
}
```

**Crear producto** — `POST /api/products` (requiere rol Admin)
```json
{
  "name": "Laptop Gamer",
  "description": "Laptop de alto rendimiento",
  "price": 1499.99,
  "stock": 10
}
```

**Actualizar producto** — `PUT /api/products/{id}` (requiere rol Admin)
```json
{
  "name": "Nombre actualizado",
  "description": "Descripción actualizada",
  "price": 59.99,
  "stock": 5
}
```

**Eliminar producto** — `DELETE /api/products/{id}` (requiere rol Admin)

**Crear orden** — `POST /api/orders` (requiere autenticación)
```json
{
  "items": [
    {
      "productId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
      "quantity": 2
    }
  ]
}
```

## Cómo ejecutar el proyecto desde cero

### Prerrequisitos

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- Git

### Pasos

1. **Clonar el repositorio**
   ```bash
   git clone <url-del-repositorio>
   cd ECommerceApi
   ```

2. **Restaurar paquetes NuGet**
   ```bash
   dotnet restore
   ```

3. **Construir la solución**
   ```bash
   dotnet build
   ```

4. **Ejecutar la API**
   ```bash
   dotnet run --project ECommerce.Api\ECommerce.Api.csproj
   ```
   La base de datos SQLite (`ecommerce.db`) se crea automáticamente al iniciar por primera vez gracias a las migraciones de EF Core.

5. **Abrir Swagger**
   ```
   https://localhost:5001/swagger
   ```
   (o la URL que muestre la consola al ejecutar)

6. **Probar los endpoints**
   - Al iniciar la API por primera vez, se crea automáticamente un usuario administrador con las siguientes credenciales:
     - **Email:** `admin@test.com`
     - **Contraseña:** `Admin123!`
   - Todos los endpoints de productos requieren autenticación. Para obtener un token JWT:
     1. Haz `POST /api/auth/login` con las credenciales de arriba.
     2. Copia el token de la respuesta.
     3. En Swagger, haz clic en **Authorize** y pega `Bearer <tu-token>`.
   - Con el token autenticado puedes probar todos los `GET`.
   - Para `POST`, `PUT` y `DELETE` de productos se necesita además el rol **Admin** (el usuario de prueba ya lo tiene).

## Base de datos

- SQLite (archivo `ecommerce.db` en la carpeta del proyecto).
- La conexión se configura en `ECommerce.Api/appsettings.json`.
- Las migraciones de EF Core están en `ECommerce.Infrastructure/Migrations/`. Se aplican automáticamente al iniciar la aplicación.
- Al iniciar por primera vez, si no existe un usuario `admin@test.com`, se crea uno con rol **Admin** automáticamente.

## Autenticación JWT

- El proyecto incluye autenticación JWT preconfigurada para desarrollo (`appsettings.json`).
- No es necesario generar certificados ni configurar claves externas.
- El token se obtiene mediante `POST /api/auth/login` y expira según el valor de `Jwt:ExpirationHours` (1 hora por defecto).

## Estructura de carpetas clave

```
ECommerce.Api/
├── Controllers/
│   ├── AuthController.cs
│   ├── OrdersController.cs
│   └── ProductsController.cs
├── Program.cs
└── appsettings.json

ECommerce.Application/
├── Behaviors/
│   └── ValidationBehavior.cs
├── DTOs/
│   ├── CreateProductDto.cs
│   ├── LoginRequestDto.cs
│   ├── ProductResponseDto.cs
│   ├── RegisterRequestDto.cs
│   └── UpdateProductDto.cs
├── Interfaces/
│   ├── IOrderRepository.cs
│   ├── IProductRepository.cs
│   ├── IProductService.cs
│   ├── ITokenService.cs
│   └── IUserRepository.cs
├── Services/
│   └── ProductService.cs
├── Auth/
│   └── Commands/
│       ├── LoginCommand.cs
│       ├── LoginCommandHandler.cs
│       ├── RegisterCommand.cs
│       └── RegisterCommandHandler.cs
├── Orders/
│   ├── Commands/
│   │   ├── CreateOrderCommand.cs
│   │   └── CreateOrderCommandHandler.cs
│   └── Validators/
│       └── CreateOrderValidator.cs
├── Products/
│   ├── Commands/
│   │   ├── CreateProductCommand.cs
│   │   ├── CreateProductCommandHandler.cs
│   │   ├── DeleteProductCommand.cs
│   │   ├── DeleteProductCommandHandler.cs
│   │   ├── UpdateProductCommand.cs
│   │   └── UpdateProductCommandHandler.cs
│   ├── Queries/
│   │   ├── GetAllProductsQuery.cs
│   │   ├── GetAllProductsQueryHandler.cs
│   │   ├── GetProductByIdQuery.cs
│   │   └── GetProductByIdQueryHandler.cs
│   └── Validators/
│       └── CreateProductValidator.cs

ECommerce.Domain/
└── Entities/
    ├── Order.cs
    ├── OrderItem.cs
    ├── OrderStatus.cs
    ├── Product.cs
    └── User.cs

ECommerce.Infrastructure/
├── Persistence/
│   ├── ApplicationDbContext.cs
│   └── Configurations/
│       ├── OrderConfiguration.cs
│       ├── OrderItemConfiguration.cs
│       ├── ProductConfiguration.cs
│       └── UserConfiguration.cs
├── Repositories/
│   ├── OrderRepository.cs
│   ├── ProductRepository.cs
│   └── UserRepository.cs
├── Services/
│   └── JwtTokenService.cs
└── Migrations/
```

## Validación con FluentValidation

Los comandos CQRS se validan automáticamente mediante un **pipeline behavior** de MediatR (`ValidationBehavior<TRequest, TResponse>`) que ejecuta los validadores de FluentValidation antes de llegar al handler.

- `CreateProductValidator` — valida nombre, precio y stock al crear un producto.
- `CreateOrderValidator` — valida que el usuario no sea vacío, que la orden tenga al menos un ítem, y que cada ítem tenga `ProductId` válido y `Quantity` entre 1 y 100.

## Dependencias principales

- `Microsoft.AspNetCore.Authentication.JwtBearer`
- `Microsoft.EntityFrameworkCore` / `Microsoft.EntityFrameworkCore.Sqlite`
- `MediatR`
- `FluentValidation` / `FluentValidation.DependencyInjectionExtensions`
- `Swashbuckle.AspNetCore`
- `BCrypt.Net-Next`
