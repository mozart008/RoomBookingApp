using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using RoomBookingApp.Core.DataServices;
using RoomBookingApp.Core.Processors;
using RoomBookingApp.Domain;
using RoomBookingApp.Persistence;
using RoomBookingApp.Persistence.Repositories;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

//set connection string
var connString = "Data Source=migration.db";
var conn = new SqliteConnection(connString);
conn.Open();
builder.Services.AddDbContext<RoomBookingAppDbContext>(options => options.UseSqlite(conn));

var contextBuilder = new DbContextOptionsBuilder<RoomBookingAppDbContext>();
contextBuilder.UseSqlite(connString);

using var context = new RoomBookingAppDbContext(contextBuilder.Options);
context.Database.EnsureCreated();
builder.Services.AddAuthorization();
builder.Services.AddControllers();
//add core services
builder.Services.AddScoped<IRoomBookingRequestProcessor, RoomBookingRequestProcessor>();
builder.Services.AddScoped<IRoomBookingService, RoomBookingService>();

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.(middleware)
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    // This creates the /swagger page and points it to that file
    //app.UseSwaggerUI(options =>
    //{
    //    options.SwaggerEndpoint("/openapi/v1.json", "v1");
    //});
    app.MapScalarApiReference(); // Accessible at /scalar/v1
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();


app.Run();
