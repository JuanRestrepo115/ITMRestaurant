using ITMRestaurant.DataAccess.Context;
using ITMRestaurant.Domain.Entities;
using ITMRestaurant.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace ITMRestaurant.DataAccess.Seeders
{
    public static class DataSeeder
    {
        public static async Task SeedAsync(RestaurantDbContext context)
        {
            // Solo ejecutar si no hay datos
            if (await context.Restaurants.AnyAsync()) return;

            // ═══ 1. RESTAURANTES ═══
            var restaurants = new List<Restaurant>
            {
                new() { Branch = "Sucursal El Poblado", Address = "Calle 10 #43-12, El Poblado", PhoneNumber = "604-111-1111", IsActive = true, CreatedAt = DateTime.UtcNow },
                new() { Branch = "Sucursal Laureles", Address = "Circular 73 #39-15, Laureles", PhoneNumber = "604-222-2222", IsActive = true, CreatedAt = DateTime.UtcNow },
                new() { Branch = "Sucursal Envigado", Address = "Calle 34 Sur #43-20, Envigado", PhoneNumber = "604-333-3333", IsActive = true, CreatedAt = DateTime.UtcNow },
            };

            context.Restaurants.AddRange(restaurants);
            await context.SaveChangesAsync();

            // ═══ 2. MESAS ═══
            var tables = new List<Table>
            {
                // Sucursal El Poblado
                new() { TableNumber = 1, RestaurantId = restaurants[0].Id, Capacity = 2, Location = "Interior", State = TableState.Available, CreatedAt = DateTime.UtcNow },
                new() { TableNumber = 2, RestaurantId = restaurants[0].Id, Capacity = 4, Location = "Interior", State = TableState.Available, CreatedAt = DateTime.UtcNow },
                new() { TableNumber = 3, RestaurantId = restaurants[0].Id, Capacity = 6, Location = "Terraza", State = TableState.Available, CreatedAt = DateTime.UtcNow },
                new() { TableNumber = 4, RestaurantId = restaurants[0].Id, Capacity = 8, Location = "Terraza", State = TableState.Available, CreatedAt = DateTime.UtcNow },

                // Sucursal Laureles
                new() { TableNumber = 5, RestaurantId = restaurants[1].Id, Capacity = 2, Location = "Interior", State = TableState.Available, CreatedAt = DateTime.UtcNow },
                new() { TableNumber = 6, RestaurantId = restaurants[1].Id, Capacity = 4, Location = "Interior", State = TableState.Available, CreatedAt = DateTime.UtcNow },
                new() { TableNumber = 7, RestaurantId = restaurants[1].Id, Capacity = 6, Location = "Terraza", State = TableState.Available, CreatedAt = DateTime.UtcNow },
                new() { TableNumber = 8, RestaurantId = restaurants[1].Id, Capacity = 8, Location = "VIP", State = TableState.Available, CreatedAt = DateTime.UtcNow },

                // Sucursal Envigado
                new() { TableNumber = 9,  RestaurantId = restaurants[2].Id, Capacity = 2, Location = "Interior", State = TableState.Available, CreatedAt = DateTime.UtcNow },
                new() { TableNumber = 10, RestaurantId = restaurants[2].Id, Capacity = 4, Location = "Interior", State = TableState.Available, CreatedAt = DateTime.UtcNow },
                new() { TableNumber = 11, RestaurantId = restaurants[2].Id, Capacity = 6, Location = "Terraza", State = TableState.Available, CreatedAt = DateTime.UtcNow },
                new() { TableNumber = 12, RestaurantId = restaurants[2].Id, Capacity = 8, Location = "VIP", State = TableState.Available, CreatedAt = DateTime.UtcNow },
            };

            context.Tables.AddRange(tables);
            await context.SaveChangesAsync();

            // ═══ 3. CLIENTES ═══
            var customers = new List<Customer>
            {
                new() { FirstName = "Carlos",   LastName = "García",    Email = "carlos.garcia@gmail.com",   PhoneNumber = "300-111-1111", CreatedAt = DateTime.UtcNow },
                new() { FirstName = "María",    LastName = "López",     Email = "maria.lopez@gmail.com",     PhoneNumber = "300-222-2222", CreatedAt = DateTime.UtcNow },
                new() { FirstName = "Andrés",   LastName = "Martínez",  Email = "andres.martinez@gmail.com", PhoneNumber = "300-333-3333", CreatedAt = DateTime.UtcNow },
                new() { FirstName = "Laura",    LastName = "Rodríguez", Email = "laura.rodriguez@gmail.com", PhoneNumber = "300-444-4444", CreatedAt = DateTime.UtcNow },
                new() { FirstName = "Santiago", LastName = "Gómez",     Email = "santiago.gomez@gmail.com",  PhoneNumber = "300-555-5555", CreatedAt = DateTime.UtcNow },
                new() { FirstName = "Valentina",LastName = "Herrera",   Email = "valentina.herrera@gmail.com",PhoneNumber = "300-666-6666", CreatedAt = DateTime.UtcNow },
            };

            context.Customers.AddRange(customers);
            await context.SaveChangesAsync();

            // ═══ 4. MENU ITEMS ═══
            var menuItems = new List<MenuItem>
            {
                // Entradas
                new() { Name = "Patacones con Hogao",   Description = "Patacones fritos con hogao casero",         Price = 12000,  Category = MenuCategory.Appetizer,   IsAvailable = true, CreatedAt = DateTime.UtcNow },
                new() { Name = "Empanadas de Pipián",   Description = "3 empanadas rellenas de pipián",            Price = 10000,  Category = MenuCategory.Appetizer,   IsAvailable = true, CreatedAt = DateTime.UtcNow },
                new() { Name = "Arepa de Chócolo",      Description = "Arepa de chócolo con queso y mantequilla",  Price = 8000,   Category = MenuCategory.Appetizer,   IsAvailable = true, CreatedAt = DateTime.UtcNow },

                // Platos Principales
                new() { Name = "Bandeja Paisa",         Description = "Frijoles, chicharrón, carne, huevo, arepa", Price = 35000,  Category = MenuCategory.MainCourse, IsAvailable = true, CreatedAt = DateTime.UtcNow },
                new() { Name = "Sancocho Trifásico",    Description = "Sancocho de res, cerdo y pollo",            Price = 28000,  Category = MenuCategory.MainCourse, IsAvailable = true, CreatedAt = DateTime.UtcNow },
                new() { Name = "Trucha a la Plancha",   Description = "Trucha con ensalada y papas al vapor",      Price = 32000,  Category = MenuCategory.MainCourse, IsAvailable = true, CreatedAt = DateTime.UtcNow },
                new() { Name = "Churrasco Parrillero",  Description = "Churrasco de 300g con chimichurri",         Price = 45000,  Category = MenuCategory.MainCourse, IsAvailable = true, CreatedAt = DateTime.UtcNow },

                // Postres
                new() { Name = "Tres Leches",           Description = "Torta tres leches con arequipe",            Price = 12000,  Category = MenuCategory.Dessert,   IsAvailable = true, CreatedAt = DateTime.UtcNow },
                new() { Name = "Flan de Caramelo",      Description = "Flan casero con caramelo",                  Price = 10000,  Category = MenuCategory.Dessert,   IsAvailable = true, CreatedAt = DateTime.UtcNow },

                // Bebidas
                new() { Name = "Limonada de Coco",      Description = "Limonada natural con coco rallado",         Price = 8000,   Category = MenuCategory.Beverage,  IsAvailable = true, CreatedAt = DateTime.UtcNow },
                new() { Name = "Jugo de Lulo",          Description = "Jugo natural de lulo",                      Price = 7000,   Category = MenuCategory.Beverage,  IsAvailable = true, CreatedAt = DateTime.UtcNow },
                new() { Name = "Agua Panela con Limón", Description = "Agua panela fría con limón",                Price = 5000,   Category = MenuCategory.Beverage,  IsAvailable = true, CreatedAt = DateTime.UtcNow },
            };

            context.MenuItems.AddRange(menuItems);
            await context.SaveChangesAsync();

            // ═══ 5. RESERVACIONES ═══
            var reservations = new List<Reservation>
            {
                new() { CustomerId = customers[0].Id, TableId = tables[0].Id, RestaurantId = restaurants[0].Id, ReservationTime = DateTime.UtcNow.AddDays(1), NumberOfGuests = 2, State = ReservationState.Confirmed,  Observations = "Sin observaciones",          CreatedAt = DateTime.UtcNow },
                new() { CustomerId = customers[1].Id, TableId = tables[1].Id, RestaurantId = restaurants[0].Id, ReservationTime = DateTime.UtcNow.AddDays(2), NumberOfGuests = 4, State = ReservationState.Pending,    Observations = "Mesa cerca a la ventana",    CreatedAt = DateTime.UtcNow },
                new() { CustomerId = customers[2].Id, TableId = tables[2].Id, RestaurantId = restaurants[0].Id, ReservationTime = DateTime.UtcNow.AddDays(3), NumberOfGuests = 6, State = ReservationState.Confirmed,  Observations = "Celebración de cumpleaños",  CreatedAt = DateTime.UtcNow },
                new() { CustomerId = customers[3].Id, TableId = tables[4].Id, RestaurantId = restaurants[1].Id, ReservationTime = DateTime.UtcNow.AddDays(1), NumberOfGuests = 2, State = ReservationState.Pending,    Observations = "Sin observaciones",          CreatedAt = DateTime.UtcNow },
                new() { CustomerId = customers[4].Id, TableId = tables[5].Id, RestaurantId = restaurants[1].Id, ReservationTime = DateTime.UtcNow.AddDays(4), NumberOfGuests = 4, State = ReservationState.Confirmed,  Observations = "Aniversario",                CreatedAt = DateTime.UtcNow },
                new() { CustomerId = customers[5].Id, TableId = tables[8].Id, RestaurantId = restaurants[2].Id, ReservationTime = DateTime.UtcNow.AddDays(2), NumberOfGuests = 2, State = ReservationState.Cancelled,  Observations = "Cancelada por el cliente",   CreatedAt = DateTime.UtcNow },
            };

            context.Reservations.AddRange(reservations);
            await context.SaveChangesAsync();

            // ═══ 6. RESERVATION DETAILS ═══
            var reservationDetails = new List<ReservationDetail>
            {
                // Reserva 1
                new() { ReservationId = reservations[0].Id, MenuItemId = menuItems[0].Id,  Quantity = 2, UnitPrice = menuItems[0].Price,  CreatedAt = DateTime.UtcNow },
                new() { ReservationId = reservations[0].Id, MenuItemId = menuItems[3].Id,  Quantity = 2, UnitPrice = menuItems[3].Price,  CreatedAt = DateTime.UtcNow },
                new() { ReservationId = reservations[0].Id, MenuItemId = menuItems[9].Id,  Quantity = 2, UnitPrice = menuItems[9].Price,  CreatedAt = DateTime.UtcNow },

                // Reserva 2
                new() { ReservationId = reservations[1].Id, MenuItemId = menuItems[1].Id,  Quantity = 4, UnitPrice = menuItems[1].Price,  CreatedAt = DateTime.UtcNow },
                new() { ReservationId = reservations[1].Id, MenuItemId = menuItems[4].Id,  Quantity = 4, UnitPrice = menuItems[4].Price,  CreatedAt = DateTime.UtcNow },
                new() { ReservationId = reservations[1].Id, MenuItemId = menuItems[10].Id, Quantity = 4, UnitPrice = menuItems[10].Price, CreatedAt = DateTime.UtcNow },

                // Reserva 3
                new() { ReservationId = reservations[2].Id, MenuItemId = menuItems[2].Id,  Quantity = 6, UnitPrice = menuItems[2].Price,  CreatedAt = DateTime.UtcNow },
                new() { ReservationId = reservations[2].Id, MenuItemId = menuItems[5].Id,  Quantity = 6, UnitPrice = menuItems[5].Price,  CreatedAt = DateTime.UtcNow },
                new() { ReservationId = reservations[2].Id, MenuItemId = menuItems[7].Id,  Quantity = 6, UnitPrice = menuItems[7].Price,  CreatedAt = DateTime.UtcNow },

                // Reserva 4
                new() { ReservationId = reservations[3].Id, MenuItemId = menuItems[0].Id,  Quantity = 2, UnitPrice = menuItems[0].Price,  CreatedAt = DateTime.UtcNow },
                new() { ReservationId = reservations[3].Id, MenuItemId = menuItems[6].Id,  Quantity = 2, UnitPrice = menuItems[6].Price,  CreatedAt = DateTime.UtcNow },
                new() { ReservationId = reservations[3].Id, MenuItemId = menuItems[11].Id, Quantity = 2, UnitPrice = menuItems[11].Price, CreatedAt = DateTime.UtcNow },

                // Reserva 5
                new() { ReservationId = reservations[4].Id, MenuItemId = menuItems[1].Id,  Quantity = 4, UnitPrice = menuItems[1].Price,  CreatedAt = DateTime.UtcNow },
                new() { ReservationId = reservations[4].Id, MenuItemId = menuItems[3].Id,  Quantity = 4, UnitPrice = menuItems[3].Price,  CreatedAt = DateTime.UtcNow },
                new() { ReservationId = reservations[4].Id, MenuItemId = menuItems[8].Id,  Quantity = 4, UnitPrice = menuItems[8].Price,  CreatedAt = DateTime.UtcNow },
            };

            context.ReservationDetails.AddRange(reservationDetails);
            await context.SaveChangesAsync();
        }
    }
}