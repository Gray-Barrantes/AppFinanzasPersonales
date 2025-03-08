using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProyectoGerencia.Data;
using ProyectoGerencia.Models;
using ProyectoGerencia.Services;
using Xunit;

namespace ProyectoGerencia.Tests
{
    public class FinancialCalculationsServiceTests
    {
        private ApplicationDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            return new ApplicationDbContext(options);
        }

        [Fact]
        public async Task GetTotalIncomeAsync_ReturnsCorrectSum()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            context.FinancialData.AddRange(
                new FinancialData { Amount = 100, Date = DateTime.Now, Category = "Ingreso" },
                new FinancialData { Amount = 200, Date = DateTime.Now, Category = "Ingreso" }
            );
            await context.SaveChangesAsync();
            var service = new FinancialCalculationsService(context);

            // Act
            var totalIncome = await service.GetTotalIncomeAsync();

            // Assert
            Assert.Equal(300, totalIncome);
        }

        [Fact]
        public async Task GetTotalExpensesAsync_ReturnsCorrectSum()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            context.FinancialData.AddRange(
                new FinancialData { Amount = -50, Date = DateTime.Now, Category = "Gasto" },
                new FinancialData { Amount = -150, Date = DateTime.Now, Category = "Gasto" }
            );
            await context.SaveChangesAsync();
            var service = new FinancialCalculationsService(context);

            // Act
            var totalExpenses = await service.GetTotalExpensesAsync();

            // Assert: La suma absoluta de gastos debe ser 200 (50 + 150)
            Assert.Equal(200, totalExpenses);
        }
    }
}
