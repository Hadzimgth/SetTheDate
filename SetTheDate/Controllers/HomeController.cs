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
        private readonly EventModelFactory _eventModelFactory;

        public HomeController(ILogger<HomeController> logger, UserModelFactory userModelFactory, EventModelFactory eventModelFactory)
        {
            _logger = logger;
            _userModelFactory = userModelFactory;
            _eventModelFactory = eventModelFactory;
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
            HttpContext.Session.SetString("Name", user.Name);
            HttpContext.Session.SetString("UserIsAdmin", user.IsAdmin.ToString());

            return RedirectToAction("Dashboard", "Home");
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
                ViewBag.Error = "Error in creating user";
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
        public async Task<IActionResult> Dashboard()
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            string name = HttpContext.Session.GetString("Name") ?? "";

            if (userId == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var dashboardModel = await _eventModelFactory.GetAllEventByUserIdAsync(userId.Value);
            dashboardModel.Name = name;

            return View(dashboardModel);
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
        public async Task<IActionResult> WeddingCard(int weddingCardId)
        {
            var weddingCard = await _eventModelFactory.GetWeddingCardByIdAsync(weddingCardId);

            return View(weddingCard);
        }
    }
}
