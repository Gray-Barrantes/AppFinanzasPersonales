using Microsoft.AspNetCore.Mvc;
using ProyectoGerencia.Models;
using ProyectoGerencia.Services;

namespace ProyectoGerencia.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GastosController : ControllerBase
    {
        private readonly GastosService _gastosService;

        public GastosController(GastosService gastosService)
        {
            _gastosService = gastosService;
        }

        [HttpPost]
        public IActionResult AddGasto([FromBody] Gasto gasto)
        {
            // Agregar gasto mediante el servicio.
            // -> Integrar SQLDatabase.AddGasto(gasto) o FirebaseHelper.AddGasto(gasto) según corresponda.
            bool resultado = _gastosService.AddGasto(gasto);

            if (resultado)
                return Ok(new { message = "Gasto registrado correctamente" });
            else
                return BadRequest(new { message = "Error al registrar gasto" });
        }

        [HttpGet("{usuarioId}")]
        public IActionResult GetGastos(string usuarioId)
        {
            // Obtener los gastos asociados al usuario.
            var gastos = _gastosService.GetGastos(usuarioId);
            return Ok(gastos);
        }
    }
}
