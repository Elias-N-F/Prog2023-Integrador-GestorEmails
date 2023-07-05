using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebApplicationEmail.Models;

namespace WebApplicationEmail.Data
{
    public class WebApplicationEmailContext : DbContext
    {
        public WebApplicationEmailContext (DbContextOptions<WebApplicationEmailContext> options)
            : base(options)
        {
        }

        public DbSet<WebApplicationEmail.Models.Correo> Correo { get; set; } = default!;

        public DbSet<WebApplicationEmail.Models.Persona> Persona { get; set; } = default!;
        public DbSet<WebApplicationEmail.Models.CorreoPersona> CorreoPersona { get; set; } = default!;
    }
}
