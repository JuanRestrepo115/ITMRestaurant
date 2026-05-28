# 🍽️ RestaurantReservation API & Client - Proyecto Final

## 📋 Descripción del Proyecto
Este proyecto es un sistema web **Full-Stack** funcional y sencillo desarrollado para la gestión inteligente de reservas y pedidos anticipados en un restaurante. La solución permite registrar clientes, administrar la disponibilidad física de las mesas en tiempo real y asociar platos o bebidas del menú a las reservas creadas.

El desarrollo backend replica de forma estricta la arquitectura desacoplada por capas, los patrones de diseño y las buenas prácticas del proyecto de referencia de clase **SportsLeague**. Por su parte, el frontend ofrece una interfaz moderna, limpia y presentable que consume la API mediante peticiones HTTP.

---

## 👥 Integrantes
* **Juan Sebastian Restrepo Caro**
---

## 🛠️ Tecnologías Utilizadas

### Backend
* **Lenguaje & Framework:** C# con .NET 8 SDK
* **Persistencia (ORM):** Entity Framework Core (Enfoque Code-First)
* **Base de Datos:** SQL Server (compatible con PostgreSQL / MySQL / SQLite)
* **Mapeo de Objetos:** AutoMapper
* **Documentación de API:** Swagger UI

### Frontend
* **En Desarrollo**


---

## 🚀 Instrucciones para Ejecutar el Proyecto

Sigue estos pasos para clonar y ejecutar el proyecto localmente en tu máquina.

### 📋 Prerrequisitos
* .NET 8 SDK instalado.
* Node.js (versión LTS recomendada para el frontend).
* Un motor de base de datos relacional (ej: SQL Server LocalDB).

### 🖥️ 1. Configuración del Backend (.NET 8)

1.  Abre una terminal y navega hasta la carpeta de la solución del backend.
2.  Restaura las dependencias de los proyectos:
    ```bash
    dotnet restore
    ```
3.  Configura la cadena de conexión en el archivo `appsettings.json` dentro de la capa **API**:
    ```json
    "ConnectionStrings": {
      "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=RestaurantReservationDb;Trusted_Connection=True;MultipleActiveResultSets=true"
    }
    ```
4.  Ejecuta las migraciones de Entity Framework Core para crear la base de datos de manera automática:
    ```bash
    dotnet ef database update --
