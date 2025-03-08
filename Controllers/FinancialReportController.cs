using Microsoft.AspNetCore.Mvc;
using ProyectoGerencia.Services;

namespace ProyectoGerencia.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FinancialReportController : ControllerBase
    {
        private readonly FinancialCalculationsService _calculationsService;

        public FinancialReportController(FinancialCalculationsService calculationsService)
        {
            _calculationsService = calculationsService;
        }

        [HttpGet("netbalance")]
        public async Task<IActionResult> GetNetBalance()
        {
            var balance = await _calculationsService.GetNetBalanceAsync();
            return Ok(balance);
        }

        [HttpGet("monthlybalance")]
        public async Task<IActionResult> GetMonthlyBalance()
        {
            var monthlyBalance = await _calculationsService.GetMonthlyNetBalanceAsync();
            return Ok(monthlyBalance);
        }

        [HttpGet("totalincome")]
        public async Task<IActionResult> GetTotalIncome()
        {
            var income = await _calculationsService.GetTotalIncomeAsync();
            return Ok(income);
        }

        [HttpGet("totalexpenses")]
        public async Task<IActionResult> GetTotalExpenses()
        {
            var expenses = await _calculationsService.GetTotalExpensesAsync();
            return Ok(expenses);
        }

        [HttpGet("averageexpenses")]
        public async Task<IActionResult> GetAverageMonthlyExpenses()
        {
            var avgExpenses = await _calculationsService.GetAverageMonthlyExpensesAsync();
            return Ok(avgExpenses);
        }

        [HttpGet("totalsbycategory")]
        public async Task<IActionResult> GetTotalsByCategory()
        {
            var totalsByCategory = await _calculationsService.GetTotalsByCategoryAsync();
            return Ok(totalsByCategory);
        }
    }
}
