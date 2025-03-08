using System;
using System.ComponentModel.DataAnnotations;
using ProyectoGerencia.Models;

namespace ProyectoGerencia.Services
{
    public class DataValidationService
    {
        /// <summary>
        /// Valida que la información de FinancialData sea correcta.
        /// </summary>
        public bool ValidateFinancialData(FinancialData data)
        {
            if (data == null)
                return false;

            // Validar que la categoría no esté vacía.
            if (string.IsNullOrWhiteSpace(data.Category))
                return false;

            // Verifica que el monto no sea cero (puedes ajustar esta validación según la lógica de negocio)
            if (data.Amount == 0)
                return false;

            // Verifica que la fecha tenga un valor válido (no sea la fecha por defecto)
            if (data.Date == default(DateTime))
                return false;

            return true;
        }
    }
}

