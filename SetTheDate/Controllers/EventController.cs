using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SetTheDate.Libraries.Dtos;
using SetTheDate.ModelFactories;
using SetTheDate.Models;

namespace SetTheDate.Controllers
{
    public class EventController: Controller
    {
        private readonly EventModelFactory _eventModelFactory;
        private readonly HttpContextAccessor _httpContextAccessor;

        public EventController(EventModelFactory eventModelFactory,
            HttpContextAccessor httpContextAccessor)
        {
            _eventModelFactory = eventModelFactory;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet]
        public IActionResult EventSetup()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(UserEventModel userVent)
        {
            int userId = _httpContextAccessor.HttpContext.Session.GetInt32("UserId") ?? 0;
            var userEventModel = await _eventModelFactory.InsertUserEventAsync(userVent, userId);

            return View(userEventModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(UserEventModel userEvent)
        {
            var userEventModel = await _eventModelFactory.UpdateUserEventAsync(userEvent);
            return View(userEventModel);

        }
    }
}
