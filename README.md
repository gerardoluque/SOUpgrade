# SOUpgrade — Sistema de Órdenes de Servicio

Migración del sistema de órdenes de servicio de **MVC 3** a una aplicación tipo **SPA (Single Page Application)** con arquitectura limpia.

| Capa | Tecnología |
|------|-----------|
| Frontend | Vue JS 3.0 + TypeScript + Vite |
| Backend | .NET 9 Web API (arquitectura limpia) |
| Base de Datos | SQL Server 2022 |
| Contenedores | Docker + Docker Compose |

---

## Arquitectura

```
SOUpgrade/
├── backend/                    # .NET 9 Web API — arquitectura limpia
│   ├── src/
│   │   ├── SOUpgrade.Domain/          # Entidades, enums, interfaces
│   │   ├── SOUpgrade.Application/     # CQRS con MediatR, DTOs, AutoMapper
│   │   ├── SOUpgrade.Infrastructure/  # EF Core, repositorios, SQL Server
│   │   └── SOUpgrade.API/             # Controladores, Swagger, Program.cs
│   └── tests/
│       ├── SOUpgrade.Domain.Tests/
│       └── SOUpgrade.Application.Tests/
│
├── frontend/                   # Vue JS 3.0 SPA
│   └── src/
│       ├── api/                # Llamadas Axios al backend
│       ├── components/         # Componentes reutilizables
│       ├── router/             # Vue Router (5 rutas)
│       ├── stores/             # Pinia (estado global)
│       ├── types/              # Tipos TypeScript
│       └── views/              # Páginas (Dashboard, Lista, Detalle, Crear, Editar)
│
├── docker-compose.yml          # Orquestación completa
└── README.md
```

---

## Inicio Rápido con Docker

### Pre-requisitos
- [Docker Desktop](https://www.docker.com/products/docker-desktop) instalado y en ejecución

### Levantar toda la aplicación

```bash
docker compose up --build
```

Una vez que los contenedores estén levantados:

| Servicio | URL |
|---------|-----|
| Frontend (Vue SPA) | http://localhost:3000 |
| Backend API | http://localhost:5000 |
| Swagger UI | http://localhost:5000 (raíz) |
| SQL Server | localhost:1433 |

---

## Desarrollo Local

### Backend (.NET 9)

**Pre-requisitos:** .NET 9 SDK, SQL Server (o Docker)

```bash
# Levantar solo SQL Server
docker compose up sqlserver -d

# Aplicar migraciones y ejecutar API
cd backend
dotnet ef database update --project src/SOUpgrade.Infrastructure --startup-project src/SOUpgrade.API
dotnet run --project src/SOUpgrade.API
```

La API queda disponible en `http://localhost:5000`.  
Swagger UI en `http://localhost:5000/`.

### Frontend (Vue 3 + Vite)

**Pre-requisitos:** Node.js 20+

```bash
cd frontend
npm install
npm run dev
```

El frontend queda disponible en `http://localhost:5173`.  
Las peticiones `/api/*` son redirigidas automáticamente al backend.

---

## API Endpoints

| Método | Ruta | Descripción |
|--------|------|-------------|
| `GET` | `/api/service-orders` | Listar todas las órdenes |
| `GET` | `/api/service-orders/{id}` | Obtener orden por ID |
| `POST` | `/api/service-orders` | Crear nueva orden |
| `PUT` | `/api/service-orders/{id}` | Actualizar orden |
| `DELETE` | `/api/service-orders/{id}` | Eliminar orden |
| `PATCH` | `/api/service-orders/{id}/status` | Cambiar estado |

### Estados disponibles
`Pending` · `InProgress` · `OnHold` · `Completed` · `Cancelled`

### Prioridades disponibles
`Low` · `Medium` · `High` · `Critical`

---

## Pruebas

### Backend
```bash
cd backend
dotnet test
```

### Frontend
```bash
cd frontend
npm run build   # build de producción + verificación de tipos
```

---

## Migraciones de Base de Datos

```bash
cd backend

# Crear nueva migración
dotnet ef migrations add <NombreMigracion> \
  --project src/SOUpgrade.Infrastructure \
  --startup-project src/SOUpgrade.API

# Aplicar migraciones
dotnet ef database update \
  --project src/SOUpgrade.Infrastructure \
  --startup-project src/SOUpgrade.API
```

---

## Tecnologías Utilizadas

### Backend
- **.NET 9** Web API
- **Entity Framework Core** (SQL Server provider)
- **MediatR** — CQRS (Commands & Queries)
- **AutoMapper** — mapeo Domain ↔ DTO
- **Swashbuckle** — documentación Swagger/OpenAPI

### Frontend
- **Vue 3** (Composition API + `<script setup>`)
- **TypeScript**
- **Vite** — bundler ultrarrápido
- **Vue Router 4** — navegación SPA
- **Pinia** — manejo de estado
- **Axios** — cliente HTTP
- **CSS Variables + Grid/Flexbox** — diseño responsivo sin framework externo
