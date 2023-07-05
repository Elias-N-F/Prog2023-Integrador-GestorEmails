namespace WebApplicationEmail.Models
{
    public class Persona
    {
        public int Id { get; set; }
        public string? Nombre { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public Correo? Correo { get; set; }
        public List<CorreoPersona> CorreoPersonas { get; } = new();
    }
}
