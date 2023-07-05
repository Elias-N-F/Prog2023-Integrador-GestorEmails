using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppEmail.Modelos
{
    public class Correo
    {
        public int Id { get; set; }
        public string? Asunto { get; set; }
        public string? Contenido { get; set; }
        public int PersonaId { get; set; }
        public Persona? Remitente { get; set; }

        public override string? ToString()
        {
            return $"Id: {this.Id}, Asunto: {this.Asunto}, Remitente: {this.Remitente.Nombre}";
        }
    }
}
