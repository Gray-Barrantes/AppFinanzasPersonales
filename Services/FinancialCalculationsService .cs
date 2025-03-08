using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProyectoGerencia.Models;
using ProyectoGerencia.Data;

namespace ProyectoGerencia.Services
{
    /// <summary>
    /// Servicio que realiza cálculos y procesamiento de datos financieros.
    /// </summary>
    public class FinancialCalculationsService
    {
        private readonly ApplicationDbContext _context;

        public FinancialCalculationsService(ApplicationDbContext context)
        {
            _context = context;
        }

        #region Funciones Básicas Existentes

        /// <summary>
        /// Calcula el total de ingresos (montos positivos).
        /// </summary>
        public async Task<decimal> GetTotalIncomeAsync()
        {
            return await _context.FinancialData
                .Where(fd => fd.Amount > 0)
                .SumAsync(fd => fd.Amount);
        }

        /// <summary>
        /// Calcula el total de gastos (montos negativos, devuelto como valor absoluto).
        /// </summary>
        public async Task<decimal> GetTotalExpensesAsync()
        {
            var totalExpenses = await _context.FinancialData
                .Where(fd => fd.Amount < 0)
                .SumAsync(fd => fd.Amount);
            return Math.Abs(totalExpenses);
        }

        /// <summary>
        /// Calcula el balance neto sumando ingresos y gastos.
        /// </summary>
        public async Task<decimal> GetNetBalanceAsync()
        {
            return await _context.FinancialData.SumAsync(fd => fd.Amount);
        }

        /// <summary>
        /// Obtiene el balance neto agrupado por mes en formato "YYYY-MM".
        /// </summary>
        public async Task<Dictionary<string, decimal>> GetMonthlyNetBalanceAsync()
        {
            var monthlyBalances = await _context.FinancialData
                .GroupBy(fd => new { fd.Date.Year, fd.Date.Month })
                .Select(g => new
                {
                    MonthKey = $"{g.Key.Year}-{g.Key.Month:D2}",
                    Balance = g.Sum(fd => fd.Amount)
                })
                .ToListAsync();

            return monthlyBalances.ToDictionary(x => x.MonthKey, x => x.Balance);
        }

        /// <summary>
        /// Calcula el promedio de gastos mensuales.
        /// </summary>
        public async Task<decimal> GetAverageMonthlyExpensesAsync()
        {
            var monthlyExpenses = await _context.FinancialData
                .Where(fd => fd.Amount < 0)
                .GroupBy(fd => new { fd.Date.Year, fd.Date.Month })
                .Select(g => Math.Abs(g.Sum(fd => fd.Amount)))
                .ToListAsync();

            return monthlyExpenses.Count > 0 ? monthlyExpenses.Average() : 0;
        }

        /// <summary>
        /// (Opcional) Calcula el total por cada categoría.
        /// Se asume que la entidad FinancialData tiene la propiedad 'Category'.
        /// </summary>
        public async Task<Dictionary<string, decimal>> GetTotalsByCategoryAsync()
        {
            var totalsByCategory = await _context.FinancialData
                .GroupBy(fd => fd.Category)
                .Select(g => new
                {
                    Category = g.Key,
                    Total = g.Sum(fd => fd.Amount)
                })
                .ToListAsync();

            return totalsByCategory.ToDictionary(x => x.Category, x => x.Total);
        }

        #endregion

        #region 1. Cálculos Estadísticos Avanzados

        /// <summary>
        /// Calcula la varianza y la desviación estándar de los gastos (valores absolutos).
        /// </summary>
        public async Task<(decimal variance, decimal stdDeviation)> GetExpenseStatisticsAsync()
        {
            var expenses = await _context.FinancialData
                .Where(fd => fd.Amount < 0)
                .Select(fd => Math.Abs(fd.Amount))
                .ToListAsync();

            if (expenses.Count == 0)
                return (0, 0);

            decimal mean = expenses.Average();
            decimal variance = expenses.Sum(x => (x - mean) * (x - mean)) / expenses.Count;
            decimal stdDev = (decimal)Math.Sqrt((double)variance);

            return (variance, stdDev);
        }

        /// <summary>
        /// Calcula la tasa de crecimiento mensual de los balances.
        /// Retorna un diccionario donde la clave es el mes (a partir del segundo mes) y el valor es la tasa de crecimiento.
        /// </summary>
        public async Task<Dictionary<string, decimal>> GetMonthlyGrowthRatesAsync()
        {
            var monthlyBalances = await GetMonthlyNetBalanceAsync();
            var sortedKeys = monthlyBalances.Keys.OrderBy(k => k).ToList();
            var growthRates = new Dictionary<string, decimal>();

            for (int i = 1; i < sortedKeys.Count; i++)
            {
                string currentMonth = sortedKeys[i];
                string previousMonth = sortedKeys[i - 1];
                decimal previousBalance = monthlyBalances[previousMonth];
                decimal growthRate = previousBalance == 0 ? 0 : (monthlyBalances[currentMonth] - previousBalance) / previousBalance;
                growthRates[currentMonth] = growthRate;
            }

            return growthRates;
        }

        #endregion

        #region 2. Pronóstico Financiero

        /// <summary>
        /// Pronostica el balance neto para el siguiente mes utilizando una regresión lineal simple basada en los balances mensuales históricos.
        /// </summary>
        public async Task<decimal> ForecastNextMonthNetBalanceAsync()
        {
            var monthlyBalances = await GetMonthlyNetBalanceAsync();
            if (monthlyBalances.Count < 2)
                return 0;

            var sortedKeys = monthlyBalances.Keys.OrderBy(k => k).ToList();
            int n = sortedKeys.Count;
            decimal sumX = 0, sumY = 0, sumXY = 0, sumX2 = 0;

            for (int i = 0; i < n; i++)
            {
                decimal x = i;
                decimal y = monthlyBalances[sortedKeys[i]];
                sumX += x;
                sumY += y;
                sumXY += x * y;
                sumX2 += x * x;
            }

            decimal slope = (n * sumXY - sumX * sumY) / (n * sumX2 - sumX * sumX);
            decimal intercept = (sumY - slope * sumX) / n;
            decimal forecast = slope * n + intercept;

            return forecast;
        }

        #endregion

        #region 3. Preparación de Datos para Visualización

        /// <summary>
        /// Prepara los datos de balance neto mensual en un formato listo para utilizar en gráficos.
        /// </summary>
        public async Task<List<ChartData>> GetMonthlyBalanceChartDataAsync()
        {
            var monthlyBalances = await GetMonthlyNetBalanceAsync();
            var chartData = monthlyBalances
                .OrderBy(kv => kv.Key)
                .Select(kv => new ChartData { Label = kv.Key, Value = kv.Value })
                .ToList();

            return chartData;
        }

        /// <summary>
        /// Prepara los totales por categoría en un formato listo para utilizar en gráficos.
        /// </summary>
        public async Task<List<ChartData>> GetTotalsByCategoryChartDataAsync()
        {
            var totalsByCategory = await GetTotalsByCategoryAsync();
            var chartData = totalsByCategory
                .Select(kv => new ChartData { Label = kv.Key, Value = kv.Value })
                .ToList();

            return chartData;
        }

        #endregion
    }
}
