using Microsoft.EntityFrameworkCore;
using Api.Models;
using Microsoft.OpenApi.Models; // 👈 necesario para la descripción de Swagger
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        options.JsonSerializerOptions.WriteIndented = true;
    });

//  Si usás .NET 8 o 9, NO uses AddOpenApi + MapOpenApi juntos con AddSwaggerGen.
// Mejor usar solo SwaggerGen (más completo y compatible con Swagger UI).
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Restaurante API",
        Version = "v1",
        Description = "API para gestionar reservas, platos y pedidos del restaurante"
    });
});

//  Configuración de la base de datos MySQL
builder.Services.AddDbContext<RestauranteDisponibilidadContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        new MySqlServerVersion(new Version(8, 0, 41)),
        mysqlOptions => mysqlOptions.EnableRetryOnFailure(
            maxRetryCount: 3,
            maxRetryDelay: TimeSpan.FromSeconds(30),
            errorNumbersToAdd: null)
    ));

//  Configurar CORS (para conexión con frontend)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

var app = builder.Build();

//  Configurar el pipeline HTTP
if (app.Environment.IsDevelopment())
{
    // Mostrar Swagger solo en desarrollo
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Restaurante API v1");
        c.RoutePrefix = string.Empty; // opcional: abre Swagger directamente
    });
}

app.UseCors("AllowAll");
app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

app.Run();
