using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplicationEmail.Data;
using WebApplicationEmail.Models;
using WebApplicationEmail.Utilities;

namespace WebApplicationEmail.Controllers
{
    public class CorreosController : Controller
    {
        private readonly WebApplicationEmailContext _context;

        public CorreosController(WebApplicationEmailContext context)
        {
            _context = context;
        }

        // GET: Correos
        [Authentication]
        public async Task<IActionResult> Index(string search, string remitente)
        {
            int id = int.Parse(HttpContext.Session.GetString("Id"));
            var webApplicationEmailContext = _context.Correo.Where(c => c.CorreoPersonas.Any(cp => cp.PersonaId == id)).Include(a => a.Remitente).Include(x => x.CorreoPersonas);
            if (!String.IsNullOrEmpty(search))
            {
                webApplicationEmailContext = _context.Correo.Where(c => c.CorreoPersonas.Any(cp => cp.PersonaId == id) && (c.Asunto.Contains(search) || c.Contenido.Contains(search))).Include(a => a.Remitente).Include(x => x.CorreoPersonas);
            }
            if (!String.IsNullOrEmpty(remitente))
            {
                webApplicationEmailContext = _context.Correo.Where(c => c.CorreoPersonas.Any(cp => cp.PersonaId == id) && c.Remitente.Nombre == remitente).Include(a => a.Remitente).Include(x => x.CorreoPersonas);
            }
            if (!String.IsNullOrEmpty(remitente) && !String.IsNullOrEmpty(search))
            {
                webApplicationEmailContext = _context.Correo.Where(c => c.CorreoPersonas.Any(cp => cp.PersonaId == id) && c.Remitente.Nombre == remitente && (c.Asunto.Contains(search) || c.Contenido.Contains(search))).Include(a => a.Remitente).Include(x => x.CorreoPersonas);
            }
            ViewData["Remi"] = new SelectList(_context.Set<Persona>(), "Nombre", "Nombre");
            ViewData["idUser"] = int.Parse(HttpContext.Session.GetString("Id"));

            var result = await webApplicationEmailContext.ToListAsync();

            var list = new HashSet<int>();
            await webApplicationEmailContext.ForEachAsync(x => list.Add(x.PersonaId));
            
            var persona = await _context.Persona.Where(p => list.Contains(p.Id)).ToListAsync();

            Dictionary<int, Persona> dict = new();

            foreach (var item in result)
            {
                var p = persona.Find(x => x.Id == item.PersonaId);
                if(p != null)
                {
                    dict.Add(item.Id, p);
                }
            }

            ViewData["personas"] = dict;
            foreach (var item in persona)
            {
                await Console.Out.WriteLineAsync(item.Id +" ,"+item.Nombre );
            }
            return View(result);
        }

        // GET: Correos/Enviados
        public async Task<IActionResult> Enviados(string search, int remitente = 0)
        {
            int id = int.Parse(HttpContext.Session.GetString("Id"));
            var webApplicationEmailContext = _context.Correo.Where(c => c.Remitente.Id == id).Include(a => a.Remitente).Include(x => x.CorreoPersonas);
            if (!String.IsNullOrEmpty(search))
            {
                webApplicationEmailContext = _context.Correo.Where(c => c.Remitente.Id == id && (c.Asunto.Contains(search) || c.Contenido.Contains(search))).Include(a => a.Remitente).Include(x => x.CorreoPersonas);
            }
            if (remitente != 0)
            {
                webApplicationEmailContext = _context.Correo.Where(c => c.Remitente.Id == id && c.CorreoPersonas.Any(cp => cp.PersonaId == remitente)).Include(a => a.Remitente).Include(x => x.CorreoPersonas);
            }
            if (remitente != 0 && !String.IsNullOrEmpty(search))
            {
                webApplicationEmailContext = _context.Correo.Where(c => c.Remitente.Id == id && c.CorreoPersonas.Any(cp => cp.PersonaId == remitente) && (c.Asunto.Contains(search) || c.Contenido.Contains(search))).Include(a => a.Remitente).Include(x => x.CorreoPersonas);
            }
            ViewData["Remi"] = new SelectList(_context.Set<Persona>(), "Id", "Nombre");

            var result = await webApplicationEmailContext.ToListAsync();

            var list = new List<int>();

            result.ForEach(correo => list.Add(correo.Id));

            var correopersona = await _context.CorreoPersona.Where(x => list.Contains(x.CorreoId)).ToListAsync();

            list.Clear();
            correopersona.ForEach(cp => list.Add(cp.PersonaId));

            var persona = await _context.Persona.Where(p => list.Contains(p.Id) || p.Id == id).ToListAsync();

            Dictionary<int, Persona> dict = new();

            foreach (var item in correopersona)
            {
                var p = persona.Find(x => x.Id == item.PersonaId);
                if (p != null)
                {
                    dict.Add(item.Id, p);
                }
                else {
                    Persona eliminado = new Persona();
                    eliminado.Nombre = "*Eliminado*";
                    dict.Add(item.Id, eliminado);
                }
            }


            ViewData["personas"] = dict;

            return View(result);
        }

        // GET: Correos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Correo == null)
            {
                return NotFound();
            }

            var correo = await _context.Correo
                .Include(c => c.Remitente)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (correo == null)
            {
                return NotFound();
            }

            int idUser = int.Parse(HttpContext.Session.GetString("Id"));

            var correoPersona = _context.CorreoPersona.Where(cp => cp.PersonaId == idUser && cp.CorreoId == correo.Id).FirstOrDefault();

            if (correoPersona != null)
            {
                correoPersona.Leido = true;
                _context.Update(correoPersona);
                await _context.SaveChangesAsync();
            }            

            return View(correo);
        }

        // GET: Correos/Create
        public IActionResult Create()
        {
            ViewData["Para"] = new SelectList(_context.Set<Persona>(), "Id", "Nombre");
            return View();
        }

        public SelectList Destinatarios { get; set; }

        // POST: Correos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Asunto,Contenido,Para")] Correo correo)
        {
            if (ModelState.IsValid)
            {
                int id = int.Parse(HttpContext.Session.GetString("Id"));
                Persona p = _context.Persona.Find(id);
                correo.Remitente = p;
                _context.Add(correo);
                await _context.SaveChangesAsync();

                foreach (int para in correo.Para)
                {
                    CorreoPersona cp = new CorreoPersona();
                    cp.CorreoId = correo.Id;
                    cp.PersonaId = para; 
                    _context.Add(cp);
                    await _context.SaveChangesAsync();
                }

                return RedirectToAction(nameof(Index));
            }
            ViewData["PersonaId"] = new SelectList(_context.Set<Persona>(), "Id", "Id", correo.PersonaId);
            return View(correo);
        }

        // GET: Correos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Correo == null)
            {
                return NotFound();
            }

            var correo = await _context.Correo.FindAsync(id);
            if (correo == null)
            {
                return NotFound();
            }
            ViewData["PersonaId"] = new SelectList(_context.Set<Persona>(), "Id", "Id", correo.PersonaId);
            return View(correo);
        }

        // POST: Correos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Asunto,Contenido,PersonaId")] Correo correo)
        {
            if (id != correo.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(correo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CorreoExists(correo.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["PersonaId"] = new SelectList(_context.Set<Persona>(), "Id", "Id", correo.PersonaId);
            return View(correo);
        }

        // GET: Correos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Correo == null)
            {
                return NotFound();
            }

            var correo = await _context.Correo
                .Include(c => c.Remitente)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (correo == null)
            {
                return NotFound();
            }

            return View(correo);
        }

        // POST: Correos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Correo == null)
            {
                return Problem("Entity set 'WebApplicationEmailContext.Correo'  is null.");
            }
            var correo = await _context.Correo.FindAsync(id);
            if (correo != null)
            {
                _context.Correo.Remove(correo);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CorreoExists(int id)
        {
          return (_context.Correo?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }





}
