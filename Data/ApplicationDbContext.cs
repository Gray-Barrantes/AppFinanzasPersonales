using Microsoft.EntityFrameworkCore;
using ProyectoGerencia.Models;


namespace ProyectoGerencia.Data
{
    public class ApplicationDbContext : DbContext
    {
        // Constructor que toma el contexto de opciones para pasar a la clase base
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // DbSet que representa la tabla de FinancialData
        public DbSet<FinancialData> FinancialData { get; set; }

        // Agregar aquí otros DbSet si hay más entidades
        // public DbSet<AnotherEntity> AnotherEntity { get; set; }

        // Pueden configurar entidades con fluidez (fluient API) si es necesario
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Ejemplo de configuración fluida, favor adaptarla según lo necesiten
            modelBuilder.Entity<FinancialData>()
                .HasKey(fd => fd.Id);  // Asegura que "Id" es la clave primaria

            // Configurar otras relaciones o restricciones si es necesario
        }
    }
}
