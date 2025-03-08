using Microsoft.EntityFrameworkCore;
using ProyectoGerencia.Models;
using ProyectoGerencia.Services;
using ProyectoGerencia.Data;

var builder = WebApplication.CreateBuilder(args);

// Cargar configuraciones desde appsettings.json
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

// Configuración de la cadena de conexión para SQL Server
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Registrar servicios del proyecto
builder.Services.AddScoped<FinancialCalculationsService>();
builder.Services.AddScoped<DataValidationService>();

// Agregar soporte para controladores (API)
builder.Services.AddControllers();

// Configurar Swagger (documentación interactiva de la API)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configuración del middleware
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

// Mapeo de rutas para los controladores
app.MapControllers();

app.Run();
