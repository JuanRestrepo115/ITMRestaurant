using ITMRestaurant.API.Mappings;
using ITMRestaurant.DataAccess.Context;
using ITMRestaurant.DataAccess.Repositories;
using ITMRestaurant.Domain.Interfaces.Repositories;
using ITMRestaurant.Domain.Interfaces.Services;
using ITMRestaurant.Domain.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// -- Entity Framework Core --

builder.Services.AddDbContext<RestaurantDbContext>(options =>

options.UseSqlServer(

builder.Configuration.GetConnectionString("DefaultConnection")));

// -- Repositories --

builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();

builder.Services.AddScoped<IMenuItemRepository, MenuItemRepository>();

builder.Services.AddScoped<IReservationRepository, ReservationRepository>();

builder.Services.AddScoped<IRestaurantRepository, RestaurantRepository>();

builder.Services.AddScoped<ITableRepository, TableRepository>();

// -- Services --

builder.Services.AddScoped<ICustomerService, CustomerService>();

builder.Services.AddScoped<IMenuItemService, MenuItemService>();

builder.Services.AddScoped<IReservationService, ReservationService>();

builder.Services.AddScoped<IRestaurantService, RestaurantService>();

builder.Services.AddScoped<ITableService, TableService>();

// -- Automapper --

builder.Services.AddAutoMapper(typeof(MappingProfile).Assembly);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
