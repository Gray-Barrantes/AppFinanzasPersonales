using ProyectoGerencia.Models;
using ProyectoGerencia.Data;
using System.Collections.Generic;

namespace ProyectoGerencia.Services
{
    public class GastosService
    {
        // Este servicio maneja la lógica relacionada con la gestión de gastos.
        // Se puede integrar SQLDatabase o FirebaseHelper según se requiera.

        // Para la simulación se utiliza una lista en memoria.
        private static List<Gasto> _gastos = new List<Gasto>();

        public bool AddGasto(Gasto gasto)
        {
            try
            {
                // Simulación: agregar gasto a la lista.
                _gastos.Add(gasto);
                // En producción, llamar a SQLDatabase.AddGasto(gasto) o similar.
                return true;
            }
            catch
            {
                return false;
            }
        }

        public List<Gasto> GetGastos(string usuarioId)
        {
            // Retornar los gastos filtrados por el identificador del usuario.
            return _gastos.FindAll(g => g.UsuarioId == usuarioId);
        }
    }
}
