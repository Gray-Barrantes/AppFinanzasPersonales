using Microsoft.AspNetCore.Mvc;
using ProyectoGerencia.Models;
using ProyectoGerencia.Services;

namespace ProyectoGerencia.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] Usuario usuario)
        {
            // Llamar a la función de registro en el servicio de autenticación.
            // -> Integrar FirebaseHelper.RegisterUser(usuario) aquí.
            bool registroExitoso = _authService.Register(usuario);

            if (registroExitoso)
                return Ok(new { message = "Usuario registrado correctamente" });
            else
                return BadRequest(new { message = "Error al registrar usuario" });
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] Usuario usuario)
        {
            // Llamar a la función de autenticación en el servicio.
            // -> Integrar FirebaseHelper.AuthenticateUser(usuario) aquí.
            string token = _authService.Login(usuario);

            if (!string.IsNullOrEmpty(token))
                return Ok(new { token = token });
            else
                return Unauthorized(new { message = "Credenciales inválidas" });
        }
    }
}
