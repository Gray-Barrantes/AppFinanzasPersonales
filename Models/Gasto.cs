using System;

namespace ProyectoGerencia.Models
{
    public class Gasto
    {
        public int Id { get; set; }
        public string UsuarioId { get; set; } // Relación del gasto con el usuario.
        public string Categoria { get; set; }
        public decimal Monto { get; set; }
        public DateTime Fecha { get; set; }
        public string Descripcion { get; set; }
    }
}
