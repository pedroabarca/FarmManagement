# FarmManagement API

Backend RESTful API para gestión de datos de granja. Actualmente soporta gestión de animales.

## 🚀 Estado: PRODUCCIÓN

**URL Producción**: https://farm-management-api-production.up.railway.app
**Base de Datos**: AWS RDS PostgreSQL (farmdb.cd2m8ewim596.us-east-2.rds.amazonaws.com)
**Plataforma**: Railway
**Estado**: ✅ Operacional

📖 **[Ver Guía de Mantenimiento →](../CLAUDE.md#maintenance-guide)**

---

## Descripción

API construida con Clean Architecture y Domain-Driven Design que provee endpoints para gestionar información de la granja. Se comunica con **farm-chat-service** para responder a consultas de usuarios vía WhatsApp.

## Tech Stack

- **Language**: C# (.NET 9.0)
- **Architecture**: Clean Architecture (DDD)
- **Patterns**: CQRS con MediatR
- **Validation**: FluentValidation
- **Logging**: Serilog
- **Database**: PostgreSQL (AWS RDS en producción, SQL Server para desarrollo local)
- **API Documentation**: Scalar API Reference (OpenAPI)
- **Deployment**: Docker + Railway

## Estructura del Proyecto

```
FarmManagement/
├── src/
│   ├── FarmManagement.API/          # Web API layer (Controllers, Middleware)
│   ├── FarmManagement.Application/  # Business logic (Commands, Queries)
│   ├── FarmManagement.Domain/       # Domain entities (Animal, etc.)
│   └── FarmManagement.Infrastructure/ # Data access, DbContext
└── tests/
    └── FarmManagement.Tests/        # Unit tests
```

## Instalación (Desarrollo Local)

```bash
cd FarmManagement
dotnet restore
dotnet build
```

## Configuración

### Producción (Railway + AWS RDS)

El servicio en producción usa:
- **Base de Datos**: PostgreSQL en AWS RDS (farmdb)
- **Variables de Entorno** (configuradas en Railway):
  ```
  DATABASE_PROVIDER=PostgreSQL
  ConnectionStrings__DefaultConnection=Host=farmdb.cd2m8ewim596.us-east-2.rds.amazonaws.com;Database=farmdb;Username=postgres;Password=***;Port=5432;SSL Mode=Require;Trust Server Certificate=true
  ASPNETCORE_ENVIRONMENT=Production
  PORT=8080
  ```

### Desarrollo Local

Para desarrollo local con SQL Server, actualiza `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost\\SQLEXPRESS;Database=FarmDB;User Id=sys_adm;Password=12345678;TrustServerCertificate=True"
  }
}
```

### Crear Base de Datos (Local)

```bash
dotnet ef database update --project src/FarmManagement.Infrastructure
```

## Ejecutar

### Desarrollo Local
```bash
dotnet run --project src/FarmManagement.API/FarmManagement.API.csproj
```

La API estará disponible en: `http://localhost:5205`

### Producción (Railway)
Ver sección de [Deployment](#deployment) más abajo.

## API Endpoints

### Animales

#### Producción
- **GET** `/api/animals` - Obtener todos los animales
  ```bash
  curl https://farm-management-api-production.up.railway.app/api/animals
  ```

- **POST** `/api/animals` - Crear un nuevo animal
  ```bash
  curl -X POST https://farm-management-api-production.up.railway.app/api/animals \
    -H "Content-Type: application/json" \
    -d '{
      "name": "Bessie",
      "species": "Cow"
    }'
  ```

#### Desarrollo Local
- **GET** `/api/animals`
  ```bash
  curl http://localhost:5205/api/animals
  ```

- **POST** `/api/animals`
  ```bash
  curl -X POST http://localhost:5205/api/animals \
    -H "Content-Type: application/json" \
    -d '{"name": "Bessie", "species": "Cow"}'
  ```

### Documentación API

#### Producción
- Scalar UI: https://farm-management-api-production.up.railway.app/scalar/v1
- OpenAPI JSON: https://farm-management-api-production.up.railway.app/openapi/v1.json

#### Desarrollo Local
- Scalar UI: `http://localhost:5205/scalar/v1`
- OpenAPI JSON: `http://localhost:5205/openapi/v1.json`

## Arquitectura y Comunicación

Este API recibe llamadas desde **farm-chat-service** cuando los usuarios hacen consultas vía WhatsApp:

```
Usuario WhatsApp (+506 8376 1070)
      │
      │ "¿Cuántos animales tengo?"
      ↓
┌──────────────────────────────────────────────────────┐
│  Meta WhatsApp Business API                          │
│  Webhook: farm-chat-service-production.up.railway.app│
└────────────┬─────────────────────────────────────────┘
             │
             │ POST /webhook
             ↓
┌──────────────────────────────────────────────────────┐
│  farm-chat-service (Railway)                         │
│  ---------------------------------------------------│
│  1. Recibe mensaje (webhook.ts)                     │
│  2. Groq AI analiza: "necesito datos"               │
│     (Llama 3.3 70B)                                 │
│  3. Genera JSON:                                    │
│     {                                               │
│       "endpoint": "/api/animals",                   │
│       "method": "GET"                               │
│     }                                               │
│  (ChatService.ts:13-41)                             │
└────────────┬────────────────────────────────────────┘
             │
             │ HTTP GET
             │ farm-management-api-production.up.railway.app/api/animals
             │ (axios call)
             ↓
┌──────────────────────────────────────────────────────┐
│  FarmManagement API (Railway) ◄── ESTE PROYECTO      │
│  ---------------------------------------------------│
│  1. AnimalsController.cs:19                         │
│  2. GetAllAnimalsQuery (MediatR)                    │
│  3. Query AWS RDS PostgreSQL                        │
└────────────┬────────────────────────────────────────┘
             │
             │ PostgreSQL Query
             ↓
┌──────────────────────────────────────────────────────┐
│  AWS RDS PostgreSQL (farmdb)                         │
│  farmdb.cd2m8ewim596.us-east-2.rds.amazonaws.com    │
└────────────┬────────────────────────────────────────┘
             │
             │ Responde JSON:
             │ [
             │   {"id":1,"name":"Bessie","species":"Cow"},
             │   {"id":2,"name":"Luna","species":"Horse"}
             │ ]
             ↓
┌──────────────────────────────────────────────────────┐
│  farm-chat-service                                   │
│  ---------------------------------------------------│
│  4. Groq AI formatea respuesta                      │
│     (ChatService.ts:76-89)                          │
│  5. "Tienes 2 animales:                             │
│     Bessie la vaca y                                │
│     Luna la yegua"                                  │
└────────────┬────────────────────────────────────────┘
             │
             │ POST whatsapp message
             ↓
      Usuario WhatsApp (+506 8376 1070)
```

## Domain Model

### Animal Entity

```csharp
public class Animal
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Species { get; set; }
    public DateTime DateOfBirth { get; set; }
}
```

## Clean Architecture Layers

### 1. Domain Layer
- Entidades del negocio (Animal)
- Interfaces de repositorios
- Sin dependencias externas

### 2. Application Layer
- Commands y Queries (CQRS)
- Handlers con MediatR
- Validadores con FluentValidation
- DTOs y mapeos

### 3. Infrastructure Layer
- DbContext de Entity Framework
- Implementación de repositorios
- Servicios externos
- Acceso a base de datos

### 4. API Layer
- Controllers
- Middlewares (ExceptionMiddleware)
- Configuración de servicios
- Punto de entrada (Program.cs)

## Testing

```bash
dotnet test
```

## Deployment

### Desplegar a Railway

1. **Instalar Railway CLI**:
   ```bash
   brew install railway
   railway login
   ```

2. **Configurar Proyecto**:
   ```bash
   cd FarmManagement
   railway init  # Si es la primera vez
   railway link  # Para linkear a proyecto existente
   ```

3. **Configurar Variables de Entorno**:
   ```bash
   railway variables --set "DATABASE_PROVIDER=PostgreSQL"
   railway variables --set "ConnectionStrings__DefaultConnection=Host=farmdb.cd2m8ewim596.us-east-2.rds.amazonaws.com;Database=farmdb;Username=postgres;Password=***;Port=5432;SSL Mode=Require;Trust Server Certificate=true"
   railway variables --set "ASPNETCORE_ENVIRONMENT=Production"
   railway variables --set "PORT=8080"
   ```

4. **Deploy**:
   ```bash
   railway up
   ```

5. **Ver Logs**:
   ```bash
   railway logs
   ```

**Nota**: El proyecto incluye un `Dockerfile` que Railway usa automáticamente para el deployment.

📖 **Ver guía completa de deployment**: [CLAUDE.md - Deployment Process](../CLAUDE.md#deployment-process)

## Mantenimiento

### Tareas Semanales
- ✅ Verificar logs en Railway Dashboard
- ✅ Monitorear uso de AWS RDS (disco, conexiones)
- ✅ Revisar Railway credits ($5/mes free tier)

### Tareas Mensuales
- ✅ Actualizar dependencias NuGet
- ✅ Revisar y limpiar datos antiguos en database
- ✅ Backup de base de datos AWS RDS

📖 **Ver guía completa de mantenimiento**: [CLAUDE.md - Maintenance Guide](../CLAUDE.md#maintenance-guide)

## Seguridad

⚠️ **IMPORTANTE**:
- El archivo `deploy.sh` contiene credenciales sensibles y está en `.gitignore`
- NUNCA commitear `deploy.sh` o archivos con credenciales al repositorio
- Las variables de entorno en Railway contienen la configuración de producción
- Rotar credenciales periódicamente (database password, tokens)

## Issues Conocidos

⚠️ **MediatR Version Mismatch**: El proyecto usa MediatR 12.4.1 con MediatR.Extensions.Microsoft.DependencyInjection 11.1.0. Se recomienda actualizar a la versión 12.x de las extensiones.

## Próximas Features

- [ ] Update y Delete de animales
- [ ] Gestión de cultivos
- [ ] Control de inventario
- [ ] Autenticación y autorización
- [ ] Paginación en consultas
- [ ] Filtros y búsqueda avanzada

## Referencias

- Documentación completa: `/claude.md` en la raíz del proyecto FARM
- [Clean Architecture](https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html)
- [MediatR Documentation](https://github.com/jbogard/MediatR)
- [FluentValidation](https://docs.fluentvalidation.net/)
