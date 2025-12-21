using System.Diagnostics;
using FluentValidation;
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
        private readonly IValidator<RegisterModel> _registerModelValidator;

        public HomeController(ILogger<HomeController> logger, UserModelFactory userModelFactory, EventModelFactory eventModelFactory, IValidator<RegisterModel> registerModelValidator)
        {
            _logger = logger;
            _userModelFactory = userModelFactory;
            _eventModelFactory = eventModelFactory;
            _registerModelValidator = registerModelValidator;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model)
        {
            var validationResult = await _userModelFactory.ValidateUser(model);

            if (!validationResult.IsSuccess)
            {
                switch (validationResult.ErrorType)
                {
                    case LoginErrorType.AccountNotFound:
                        ViewBag.Error = "Account does not exist. Please check your email or register a new account.";
                        break;
                    case LoginErrorType.WrongPassword:
                        ViewBag.Error = "Incorrect password. Please try again.";
                        break;
                    default:
                        ViewBag.Error = "Invalid username or password";
                        break;
                }
                return View(model);
            }

            // Store session
            HttpContext.Session.SetInt32("UserId", validationResult.User.Id);
            HttpContext.Session.SetString("UserName", validationResult.User.Email);
            HttpContext.Session.SetString("Name", validationResult.User.Name);
            HttpContext.Session.SetString("UserIsAdmin", validationResult.User.IsAdmin.ToString());

            return RedirectToAction("Dashboard", "Home");
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            var validationResult = await _registerModelValidator.ValidateAsync(model);

            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }
                return View(model);
            }

            var user = await _userModelFactory.RegisterUser(model);

            if (user == null)
            {
                ViewBag.Error = "Error in creating user";
                return View(model);
            }

            // Store session
            HttpContext.Session.SetInt32("UserId", user.Id);
            HttpContext.Session.SetString("UserName", user.Email);
            HttpContext.Session.SetString("UserIsAdmin", user.IsAdmin.ToString());

            return RedirectToAction("Login", "Home");
        }

        [HttpPost]
        public IActionResult Logout()
        {
            // Clear all session data
            HttpContext.Session.Clear();

            // Redirect to login page
            return RedirectToAction("Login", "Home");
        }

        public IActionResult Index()
        {
            return RedirectToAction("Login", "Home");
        }
        public async Task<IActionResult> Dashboard()
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            string name = HttpContext.Session.GetString("Name") ?? "";

            if (userId == null)
            {
                return RedirectToAction("Login", "Home");
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
