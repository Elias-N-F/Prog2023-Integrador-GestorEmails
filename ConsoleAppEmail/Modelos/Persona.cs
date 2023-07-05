using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppEmail.Modelos
{

    public class Persona
    {
        public Persona() { }
        public int? Id { get; set; }
        public string? Nombre { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }

        public override string? ToString()
        {
            return $"Id: {Id}, Nombre: {Nombre}, Email: {Email}";
        }
    }
}
