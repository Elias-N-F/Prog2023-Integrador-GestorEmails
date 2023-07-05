using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using WebApplicationEmail.Data;
using WebApplicationEmail.Models;
using WebApplicationEmail.Utilities;

namespace WebApplicationEmail.Controllers
{
    public class HomeController : Controller
    {
        private readonly WebApplicationEmailContext _context;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, WebApplicationEmailContext context)
        {
            _logger = logger;
            _context = context;
        }

        // Get Action
        public IActionResult Login()
        {
            if (HttpContext.Session.GetString("Name") == null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index");
            }
        }
        //Post Action
        [HttpPost]
        public ActionResult Login(Persona credentials)
        {
            string pass = credentials.Password = Encrypter.EncryptPassword(credentials.Password, credentials.Email);

            bool userExist = _context.Persona.Any(x => x.Email == credentials.Email && x.Password == pass);
            if (userExist)
            {
                Persona persona = _context.Persona.First(x => x.Email == credentials.Email);
                HttpContext.Session.SetString("Name", persona.Nombre);
                HttpContext.Session.SetString("Id", persona.Id.ToString());
                return RedirectToAction("Index", "Correos");
            }
            else
            {
                return RedirectToAction("Login");
            }
        }

        [Authentication]
        public IActionResult Index()
        {
            return RedirectToAction("Index", "Correos");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public ActionResult Logout()
        {
            HttpContext.Session.Clear();
            HttpContext.Session.Remove("UserName");
            return RedirectToAction("Login");
        }
    }
}