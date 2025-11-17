using System;
using Microsoft.EntityFrameworkCore;
using Reservations.Api.Data;
using Reservations.Api.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// DB
var conn = builder.Configuration.GetConnectionString("Default") ?? "Data Source=reservations.db";
builder.Services.AddDbContext<AppDbContext>(opt => opt.UseSqlite(conn));

// Services
builder.Services.AddScoped<IReservationService, ReservationService>();

// CORS
builder.Services.AddCors(o => o.AddPolicy("AllowAll", p => p.AllowAnyMethod().AllowAnyHeader().AllowAnyOrigin()));

var app = builder.Build();

// Ensure DB
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

// Middleware
//if (app.Environment.IsDevelopment())
//{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Reservations API V1");
        c.RoutePrefix = "swagger"; // rota /swagger
    });
//}

app.UseCors("AllowAll");
app.UseAuthorization();
app.MapControllers();
app.Run();