# Sistema de Gestión de Restaurante

Sistema completo de gestión de restaurante con backend .NET 9 y frontend React.

## Estructura del Proyecto

```
RestauranteP2/
├── Api/              # Backend .NET 9 con Entity Framework Core
├── FrontEnd/         # Frontend React con Vite
└── Script_SQL/       # Scripts de base de datos
```

## Tecnologías

### Backend
- .NET 9.0
- Entity Framework Core
- MySQL (Pomelo.EntityFrameworkCore.MySql)
- ASP.NET Core Web API
- Swagger/OpenAPI

### Frontend
- React 19
- React Router DOM
- Axios
- Vite

## Configuración Inicial

### Backend (API)

1. Asegúrate de tener MySQL corriendo en `localhost:3306`
2. Actualiza `appsettings.json` con tus credenciales de MySQL:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "server=localhost;database=RestauranteDisponibilidad;user=root;password=TU_PASSWORD;port=3306"
  }
}
```

3. Ejecuta las migraciones o el script SQL para crear la base de datos:
```bash
cd Api
dotnet ef database update
# O ejecuta el script SQL manualmente
```

4. Ejecuta la API:
```bash
cd Api
dotnet run
```

La API estará disponible en:
- HTTP: `http://localhost:5142`
- HTTPS: `https://localhost:7099`
- Swagger: `http://localhost:5142` (en modo desarrollo)

### Frontend

1. Instala las dependencias:
```bash
cd FrontEnd/Restaurante
npm install
```

2. Asegúrate de que el backend esté corriendo

3. Ejecuta el frontend:
```bash
npm run dev
```

El frontend estará disponible en `http://localhost:3000`

## Funcionalidades

### Backend API

- ✅ Gestión de Platos (CRUD)
- ✅ Gestión de Categorías
- ✅ Gestión de Reservas
- ✅ Gestión de Usuarios
- ✅ Gestión de Mesas
- ✅ CORS configurado
- ✅ Swagger para documentación

### Frontend

- ✅ Página de inicio con platos destacados
- ✅ Menú completo con filtros y búsqueda
- ✅ Sistema de reservas
- ✅ Registro e inicio de sesión
- ✅ Ver mis reservas
- ✅ Diseño responsive y moderno

## Endpoints Principales

### Platos
- `GET /api/platos` - Listar todos los platos
- `GET /api/platos/{id}` - Obtener un plato
- `POST /api/platos` - Crear plato
- `PUT /api/platos/{id}` - Actualizar plato
- `DELETE /api/platos/{id}` - Eliminar plato (lógico)

### Reservas
- `GET /api/reservas` - Listar todas las reservas
- `GET /api/reservas/{id}` - Obtener una reserva
- `GET /api/reservas/usuario/{usuarioId}` - Reservas de un usuario
- `POST /api/reservas` - Crear reserva
- `PUT /api/reservas/{id}/estado` - Actualizar estado

### Usuarios
- `GET /api/usuarios` - Listar usuarios
- `GET /api/usuarios/{id}` - Obtener usuario
- `POST /api/usuarios` - Crear usuario
- `PUT /api/usuarios/{id}` - Actualizar usuario
- `DELETE /api/usuarios/{id}` - Eliminar usuario (lógico)

### Mesas
- `GET /api/mesas` - Listar mesas
- `POST /api/mesas` - Crear mesa
- `PUT /api/mesas/{id}` - Actualizar mesa
- `DELETE /api/mesas/{id}` - Eliminar mesa

### Categorías
- `GET /api/categoriaplato` - Listar categorías
- `POST /api/categoriaplato` - Crear categoría
- `PUT /api/categoriaplato/{id}` - Actualizar categoría
- `DELETE /api/categoriaplato/{id}` - Eliminar categoría

## Notas

- El frontend está configurado para conectarse al backend en `http://localhost:5142`
- La autenticación actualmente es básica (almacenamiento en localStorage)
- El backend usa camelCase para serialización JSON
- CORS está configurado para permitir todas las conexiones (ajustar en producción)

## Desarrollo

Para desarrollo simultáneo de backend y frontend:

1. Terminal 1 - Backend:
```bash
cd Api
dotnet watch run
```

2. Terminal 2 - Frontend:
```bash
cd FrontEnd/Restaurante
npm run dev
```

¡Listo! El sistema está completamente funcional.
