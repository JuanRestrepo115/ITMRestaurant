# 🍽️ ITM Restaurant — Sistema de Reservas

## 📋 Descripción del Proyecto
Este proyecto es un sistema web **Full-Stack** funcional desarrollado para la gestión inteligente de reservas y pedidos anticipados en un restaurante. La solución permite registrar clientes, administrar la disponibilidad física de las mesas en tiempo real y asociar platos o bebidas del menú a las reservas creadas.

El desarrollo backend replica de forma estricta la arquitectura desacoplada por capas, los patrones de diseño y las buenas prácticas del proyecto de referencia de clase **SportsLeague**. Por su parte, el frontend ofrece una interfaz moderna, limpia y presentable que consume la API mediante peticiones HTTP.

---

## 👥 Integrantes
* **Juan Sebastian Restrepo Caro**

---

## 🛠️ Tecnologías Utilizadas

### Backend
* **Lenguaje & Framework:** C# con .NET 8 SDK
* **Persistencia (ORM):** Entity Framework Core 8 (Enfoque Code-First con Migraciones)
* **Base de Datos:** SQL Server
* **Mapeo de Objetos:** AutoMapper 12
* **Documentación de API:** Swagger UI (Swashbuckle)

### Frontend
* **Framework:** Angular 19
* **Lenguaje:** TypeScript
* **Estilos:** SCSS
* **Tipografía:** Montserrat (Google Fonts)

---

## 🏗️ Arquitectura del Backend

El backend está dividido en **3 capas independientes**:

```
ITMRestaurant.Domain      → Entidades, Interfaces, Servicios, DTOs, Enums
ITMRestaurant.DataAccess  → DbContext, Repositorios, Migraciones, DataSeeder
ITMRestaurant.API         → Controllers, AutoMapper, Program.cs
```

### Patrones Implementados
* **Repository Pattern** — `GenericRepository<T>` + repositorios específicos
* **Service Layer** — Lógica de negocio y validaciones en la capa Domain
* **DTOs** — `RequestDTO` y `ResponseDTO` por entidad, nunca se exponen entidades directamente
* **AutoMapper** — Mapeo automático entre entidades y DTOs
* **DataSeeder** — Población automática de datos al iniciar la aplicación

---

## 📦 Modelo de Dominio

### Entidades
| Entidad | Descripción |
|---|---|
| `Customer` | Cliente con datos de contacto |
| `Restaurant` | Sucursal del restaurante |
| `Table` | Mesa con capacidad y ubicación |
| `MenuItem` | Plato o bebida del menú |
| `Reservation` | Reserva con fecha, hora y estado |
| `ReservationDetail` | Tabla intermedia N:M entre Reservation y MenuItem |

### Enums
| Enum | Valores |
|---|---|
| `TableState` | Available, Reserved, Occupied, UnderMaintenance |
| `ReservationState` | Pending, Confirmed, Cancelled, Completed |
| `MenuCategory` | Starter, MainCourse, Dessert, Beverage |

### Relaciones
* `Restaurant` 1:N `Table`
* `Restaurant` 1:N `Reservation`
* `Customer` 1:N `Reservation`
* `Table` 1:N `Reservation`
* `Reservation` N:M `MenuItem` — vía `ReservationDetail`

---

## 🌐 Endpoints de la API

### Customer
| Método | Ruta | Descripción |
|---|---|---|
| GET | `/api/customer` | Todos los clientes |
| GET | `/api/customer/{id}` | Cliente por ID |
| GET | `/api/customer/with-reservations` | Clientes con reservas |
| POST | `/api/customer` | Crear cliente |
| PUT | `/api/customer/{id}` | Editar cliente |
| DELETE | `/api/customer/{id}` | Eliminar cliente |

### Restaurant
| Método | Ruta | Descripción |
|---|---|---|
| GET | `/api/restaurant` | Todos los restaurantes |
| GET | `/api/restaurant/{id}` | Restaurante por ID |
| GET | `/api/restaurant/active` | Restaurantes activos |
| POST | `/api/restaurant` | Crear restaurante |
| PUT | `/api/restaurant/{id}` | Editar restaurante |
| PATCH | `/api/restaurant/{id}/active` | Activar/desactivar |
| DELETE | `/api/restaurant/{id}` | Eliminar restaurante |

### Table
| Método | Ruta | Descripción |
|---|---|---|
| GET | `/api/table` | Todas las mesas |
| GET | `/api/table/{id}` | Mesa por ID |
| GET | `/api/table/state/{state}` | Mesas por estado |
| POST | `/api/table` | Crear mesa |
| PUT | `/api/table/{id}` | Editar mesa |
| PATCH | `/api/table/{id}/state` | Cambiar estado |
| DELETE | `/api/table/{id}` | Eliminar mesa |

### MenuItem
| Método | Ruta | Descripción |
|---|---|---|
| GET | `/api/menuitem` | Todos los items |
| GET | `/api/menuitem/{id}` | Item por ID |
| GET | `/api/menuitem/available` | Items disponibles |
| GET | `/api/menuitem/category/{category}` | Items por categoría |
| GET | `/api/menuitem/price-range` | Items por rango de precio |
| POST | `/api/menuitem` | Crear item |
| PUT | `/api/menuitem/{id}` | Editar item |
| PATCH | `/api/menuitem/{id}/availability` | Cambiar disponibilidad |
| DELETE | `/api/menuitem/{id}` | Eliminar item |

### Reservation
| Método | Ruta | Descripción |
|---|---|---|
| GET | `/api/reservation` | Todas las reservas |
| GET | `/api/reservation/{id}` | Reserva por ID |
| GET | `/api/reservation/state/{state}` | Reservas por estado |
| GET | `/api/reservation/date-range` | Reservas por rango de fechas |
| POST | `/api/reservation` | Crear reserva |
| PUT | `/api/reservation/{id}` | Editar reserva |
| PATCH | `/api/reservation/{id}/state` | Cambiar estado |
| DELETE | `/api/reservation/{id}` | Eliminar reserva |

### ReservationDetail
| Método | Ruta | Descripción |
|---|---|---|
| GET | `/api/reservationdetail` | Todos los detalles |
| GET | `/api/reservationdetail/{id}` | Detalle por ID |
| POST | `/api/reservationdetail` | Agregar detalle |
| PUT | `/api/reservationdetail/{id}` | Editar detalle |
| DELETE | `/api/reservationdetail/{id}` | Eliminar detalle |

---

## 🚀 Instrucciones para Ejecutar el Proyecto

### 📋 Prerrequisitos
* .NET 8 SDK instalado
* Node.js LTS instalado
* Angular CLI instalado (`npm install -g @angular/cli`)
* SQL Server instalado y corriendo

### 🖥️ 1. Configuración del Backend

1. Clona el repositorio y navega a la carpeta del backend:
    ```bash
    cd ITMRestaurant
    ```

2. Restaura las dependencias:
    ```bash
    dotnet restore
    ```

3. Configura la cadena de conexión en `ITMRestaurant.API/appsettings.json`:
    ```json
    "ConnectionStrings": {
      "DefaultConnection": "Server=localhost;Database=ITMRestaurantDb;Trusted_Connection=true;TrustServerCertificate=true;"
    }
    ```

4. Ejecuta las migraciones para crear la base de datos:
    ```bash
    dotnet ef database update --project ITMRestaurant.DataAccess --startup-project ITMRestaurant.API
    ```

5. Ejecuta el backend:
    ```bash
    dotnet run --project ITMRestaurant.API
    ```

6. Abre Swagger en: `https://localhost:7096/swagger`

> **Nota:** El DataSeeder pobla automáticamente la BD con datos de prueba al primer arranque.

### 🌐 2. Configuración del Frontend

1. Navega a la carpeta del frontend:
    ```bash
    cd itm-restaurant-frontend
    ```

2. Instala las dependencias:
    ```bash
    npm install
    ```

3. Ejecuta el frontend:
    ```bash
    ng serve
    ```

4. Abre la aplicación en: `http://localhost:4200`

> **Nota:** Asegúrate de que el backend esté corriendo antes de iniciar el frontend.

---

## 📱 Vistas del Frontend

| Vista | Ruta | Descripción |
|---|---|---|
| Listado | `/reservations` | Lista todas las reservaciones con acciones |
| Formulario | `/reservations/create` | Crear nueva reservación con platos |
| Edición | `/reservations/edit/:id` | Editar reservación existente |
| Detalle | `/reservations/:id` | Ver detalle completo con platos y total |
