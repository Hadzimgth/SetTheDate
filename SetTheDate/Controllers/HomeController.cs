using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SetTheDate.Models;

namespace SetTheDate.Controllers
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
        public IActionResult Dashboard()
        {
            return View();
        }
        public IActionResult EventSetup()
        {
            return View();
        }
        public IActionResult GuestSetup()
        {
            return View();
        }
        public IActionResult GuestQuestion()
        {
            return View();
        }
        public IActionResult EventSetupSummary()
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
    }
}
