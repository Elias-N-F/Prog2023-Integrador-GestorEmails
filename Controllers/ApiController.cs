using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Nodes;
using WebApplicationEmail.Data;
using WebApplicationEmail.Models;
using WebApplicationEmail.Utilities;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplicationEmail.Controllers
{
    [Route("api/")]
    [ApiController]
    public class ApiController : Controller
    {

        private readonly WebApplicationEmailContext _context;

        public ApiController(WebApplicationEmailContext context)
        {
            _context = context;
        }

        [HttpPost]
        [Route("login/")]
        [Consumes("application/json")]
        public ActionResult Login(JsonObject json)
        {

            Persona credentials = new();
            if (json.ContainsKey("email"))
            {
                credentials.Email = json["email"].GetValue<string>();
            }
            if (json.ContainsKey("password"))
            {
                credentials.Password = json["password"].GetValue<string>();

                credentials.Password = Encrypter.EncryptPassword(credentials.Password, credentials.Email);
            }
            bool userExist = _context.Persona.Any(x => x.Email == credentials.Email && x.Password == credentials.Password);
            if (userExist)
            {
                Persona persona = _context.Persona.First(x => x.Email == credentials.Email);
                HttpContext.Session.SetString("Name", persona.Nombre);
                HttpContext.Session.SetString("Id", persona.Id.ToString());
                return Json(HttpContext.Session);
            }
            else
            {
                return NotFound("Error en el login");
            }
        }

        // GET: api/<ValuesController>
        [HttpGet]
        [Route("persona/")]
        [Authentication]
        public async Task<IActionResult> GetPersonas()
        {
            return _context.Persona != null ?
                                       Json(await _context.Persona.ToListAsync()) :
                                       Json("Entity set 'WebApplicationEmailContext.Persona' is null.");

        }

        // GET api/<ValuesController>/5
        [HttpGet]
        [Route("persona/{id}")]
        [Authentication]
        public async Task<IActionResult> GetPersona(int id)
        {
            if (id == null || _context.Persona == null)
            {
                return NotFound();
            }

            var persona = await _context.Persona
                .FirstOrDefaultAsync(m => m.Id == id);
            if (persona == null)
            {
                return NotFound();
            }

            return Json(persona);
        }

        [HttpGet]
        [Route("correo/")]
        [Authentication]
        public async Task<IActionResult> GetCorreos()
        {
            return _context.Correo != null ?
                                      Json(await _context.Correo.ToListAsync()) :
                                      Json("Entity set 'WebApplicationEmailContext.Persona' is null.");
        }

        // GET api/<ValuesController>/5
        [HttpGet]
        [Route("correo/{id}")]
        [Authentication]
        public async Task<IActionResult> GetCorreo(int id)
        {
            if (id == null || _context.Correo == null)
            {
                return NotFound();
            }

            var correo = await _context.Correo
                .FirstOrDefaultAsync(m => m.Id == id);
            if (correo == null)
            {
                return NotFound();
            }

            return Json(correo);
        }

        // POST api/<ValuesController>
        [HttpPost]
        [Route("persona/")]
        [Consumes("application/json")]
        [Authentication]
        public async Task<IActionResult> PostCreatePersona(JsonObject json)
        {
            Persona p = new();

            if (json.ContainsKey("email"))
            {
                p.Email = json["email"].GetValue<string>();
            }
            if (json.ContainsKey("password"))
            {
                p.Password = json["password"].GetValue<string>();
                p.Password = Encrypter.EncryptPassword(p.Password, p.Email);

            }
            if (json.ContainsKey("nombre"))
            {
                p.Nombre = json["nombre"].GetValue<string>();
            }
            try
            {
                _context.Add(p);
                await _context.SaveChangesAsync();
                return Json(p);
            }
            catch (Exception)
            {
                return BadRequest();
            }
                        
        }

        // DELETE api/<ValuesController>/5
        [HttpDelete]
        [Route("persona/{id}")]
        [Authentication]
        public async Task<IActionResult> DeletePersona(int id)
        {

            var persona = await _context.Persona
                .FirstOrDefaultAsync(m => m.Id == id);
            if (persona == null)
            {
                return NotFound();
            }

            if (_context.Persona == null)
            {
                return Problem("Entity set 'WebApplicationEmailContext.Persona'  is null.");
            }
            persona = await _context.Persona.FindAsync(id);
            if (persona != null)
            {
                _context.Persona.Remove(persona);
            }

            await _context.SaveChangesAsync();
            return Json("Eliminado correctamente");



            //var persona = await _context.Persona.FindAsync(id);

            //if (persona == null)
            //{
            //    return NotFound();
            //}
            //await _context.CorreoPersona.Where(x => x.PersonaId == id).ExecuteDeleteAsync();
            //_context.Persona.Remove(persona);
            //await _context.SaveChangesAsync();
            //return Json("Eliminado correctamente");

        }
    }
}
