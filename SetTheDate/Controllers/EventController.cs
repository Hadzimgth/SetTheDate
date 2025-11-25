using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using PhoneNumbers;
using Repository;
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

        [HttpPost]
        public async Task<IActionResult> GuestSetup(EventGuestListModel model)
        {
            if (model.GuestFile == null || model.GuestFile.Length == 0)
            {
                ModelState.AddModelError("", "Please upload a valid .xlsx file.");
                return View();
            }

            var guests = new List<EventGuestModel>();

            using (var stream = new MemoryStream())
            {
                await model.GuestFile.CopyToAsync(stream);

                using (var package = new ExcelPackage(stream))
                {
                    var worksheet = package.Workbook.Worksheets.First();
                    var rowCount = worksheet.Dimension.Rows;

                    for (int row = 2; row <= rowCount; row++)
                    {
                        var guest = new EventGuestModel
                        {
                            GuestName = worksheet.Cells[row, 1].GetValue<string>(),
                            PhoneNumber = worksheet.Cells[row, 2].GetValue<string>(),
                            IsValid = _eventModelFactory.ValidateMobile(worksheet.Cells[row, 2].GetValue<string>()),
                            UserEventId = model.UserEventId
                        };

                        guests.Add(guest);
                    }
                }
            }

            model.eventGuestList = guests;

            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> SaveGuestList(EventGuestListModel model)
        {
            if (model?.eventGuestList == null || !model.eventGuestList.Any())
            {
                ModelState.AddModelError("", "No guest data to save.");
                return View("GuestSetup", model);
            }

            _eventModelFactory.InsertGuestListAsync(model.eventGuestList);

            return RedirectToAction();
        }

        public IActionResult DownloadGuestTemplate()
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "templates", "GuestTemplate.xlsx");

            if (!System.IO.File.Exists(filePath))
                return NotFound("Template not found.");

            var fileBytes = System.IO.File.ReadAllBytes(filePath);
            var fileName = "GuestTemplate.xlsx";

            return File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }
    }
}
