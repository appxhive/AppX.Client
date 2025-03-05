using AppX.TestClient.B2CApp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace AppX.TestClient.B2CApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
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

        public IActionResult SignIn()
        {
            return View();
        }

        public new IActionResult SignOut()
        {
            return View();
        }

        public IActionResult EditProfile()
        {
            return View();
        }
    }
}
