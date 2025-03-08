using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProyectoGerencia.Models
{
    public class FinancialData
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public decimal Amount { get; set; } // Monto positivo (ingreso) o negativo (gasto)

        [Required]
        public DateTime Date { get; set; } // Fecha del registro

        [Required]
        [MaxLength(100)]
        public string Description { get; set; } // Descripción del ingreso/gasto

        [Required]
        [MaxLength(50)]
        public string Category { get; set; } // Nueva propiedad: Categoría del gasto o ingreso

        [Required]
        public string UserId { get; set; } // Usuario que registró la transacción
    }
}
