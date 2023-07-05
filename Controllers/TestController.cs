using Microsoft.AspNetCore.Mvc;
using System.Text.Encodings.Web;

namespace WebApplicationEmail.Controllers;

public class TestController : Controller
{
    // 
    // GET: /Test/
    public IActionResult Index()
    {
        return View();
    }
    // 
    // GET: /Test/Welcome/ 
    public string Welcome()
    {
        return "This is the Welcome action method...";
    }

    // GET: /Test/Personalizado/ 
    public IActionResult Personalizado(string name, int numTimes = 1)
    {
        ViewData["Message"] = "Hello " + name;
        ViewData["NumTimes"] = numTimes;
        return View();
    }
}