
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplicationEmail.Models
{
    public class Correo
    {
        public int Id { get; set; }
        public string? Asunto { get; set; }
        public string? Contenido { get; set; }
        public int PersonaId { get; set; }
        public Persona? Remitente { get; set; }
        public List<CorreoPersona> CorreoPersonas { get; } = new();
        [BindProperty]
        [NotMapped]
        public int[] Para { get; set; }
    }
}
