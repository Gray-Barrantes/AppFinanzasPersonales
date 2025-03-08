namespace ProyectoGerencia.Models
{
    public class Usuario
    {
        public string Id { get; set; }  // Ejemplo: asignado por Firebase.
        public string Nombre { get; set; }
        public string Email { get; set; }
        public string Password { get; set; } // Nota: En producción, no almacenar contraseñas en texto plano.
    }
}
