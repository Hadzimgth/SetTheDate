using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SetTheDate.ModelFactories;
using SetTheDate.Models;

namespace SetTheDate.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserModelFactory _userModelFactory;

        public HomeController(ILogger<HomeController> logger, UserModelFactory userModelFactory)
        {
            _logger = logger;
            _userModelFactory = userModelFactory;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model)
        {
            var user = await _userModelFactory.ValidateUser(model);

            if (user == null)
            {
                ViewBag.Error = "Invalid username or password";
                return View();
            }

            // Store session
            HttpContext.Session.SetInt32("UserId", user.Id);
            HttpContext.Session.SetString("UserName", user.Email);
            HttpContext.Session.SetString("UserIsAdmin", user.IsAdmin.ToString());

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        public async Task<IActionResult> Register(RegisterModel model)
        {
            var user = await _userModelFactory.RegisterUser(model);

            if (user == null)
            {
                ViewBag.Error = "Error";
                return View();
            }

            // Store session
            HttpContext.Session.SetInt32("UserId", user.Id);
            HttpContext.Session.SetString("UserName", user.Email);
            HttpContext.Session.SetString("UserIsAdmin", user.IsAdmin.ToString());

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public IActionResult Logout()
        {
            // Clear all session data
            HttpContext.Session.Clear();

            // Redirect to home or login page
            return RedirectToAction("Index", "Home");
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
            var tempModel = new EventSurveySetup();
            return View(tempModel);
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

        [HttpGet]
        public IActionResult WeddingCard()
        {
            var weddingCard = new WeddingCardInformationModel();
            weddingCard.WeddingCardType = 1;

            return View(weddingCard);
        }
    }
}
