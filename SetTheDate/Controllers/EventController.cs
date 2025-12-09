using ExcelDataReader;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        public EventController(EventModelFactory eventModelFactory)
        {
            _eventModelFactory = eventModelFactory;
        }

        [HttpGet]
        public async Task<IActionResult> EventSetup(int? eventId)
        {
            int userId = HttpContext.Session.GetInt32("UserId") ?? 0;

            var userEvent = new UserEventModel
            {
                UserId = userId,
                EventDate = DateTime.Now
            };

            if(eventId.HasValue && eventId > 0)
            {
                userEvent = await _eventModelFactory.GetEventAndWeddingCardByIdAsync(eventId.Value);
            }

            return View(userEvent);
        }

        [HttpPost]
        public async Task<IActionResult> Create(UserEventModel userEvent)
        {
            var userEventModel = await _eventModelFactory.InsertUserEventAsync(userEvent);

            return RedirectToAction("GuestQuestion", new { id = userEventModel.Id });
        }

        [HttpGet]
        public async Task<IActionResult> GuestQuestion(int id)
        {
            var eventSurvey = new EventSurveySetup();
            eventSurvey.UserEventId = id;
            var answerList = new List<EventAnswerModel>();
            var answer = new EventAnswerModel();
            answer.Answer = "Nasi Ayam";
            answerList.Add(answer);
            answer = new EventAnswerModel();
            answer.Answer = "Nasi Goreng";
            answerList.Add(answer);

            var eventQuestionModel = new EventQuestionModel();
            eventQuestionModel.Question = "What would you like to have at the wedding?";
            eventQuestionModel.EventAnswerListModel = answerList;

            eventSurvey.EventQuestionListModel.Add(eventQuestionModel);

            return View(eventSurvey);
        }

        [HttpPost]
        public async Task<IActionResult> GuesQuestionCreate(EventSurveySetup surverySetup)
        {
            await _eventModelFactory.InsertEventQuestionListAsync(surverySetup);

            return RedirectToAction("GuestSetup", new { id = surverySetup.UserEventId });
        }

        [HttpGet]
        public async Task<IActionResult> GuestSetup(int id)
        {
            var guestSetup = new EventGuestListModel();
            guestSetup.UserEventId = id;

            return View(guestSetup);
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
                return View(model);
            }

            var guests = new List<EventGuestModel>();

            using (var stream = model.GuestFile.OpenReadStream())
            using (var reader = ExcelDataReader.ExcelReaderFactory.CreateReader(stream))
            {
                var result = reader.AsDataSet();

                var table = result.Tables[0];

                if (table.Rows.Count <= 1)
                {
                    ModelState.AddModelError("", "There is no data in the excel file");
                    return View(model);
                }

                for (int row = 1; row < table.Rows.Count; row++)
                {
                    var name = table.Rows[row][0].ToString();
                    var phone = table.Rows[row][1].ToString();

                    guests.Add(new EventGuestModel
                    {
                        GuestName = name,
                        PhoneNumber = phone,
                        IsValid = _eventModelFactory.ValidateMobile(phone),
                        UserEventId = model.UserEventId
                    });
                }
            }

            model.eventGuestList = guests;
            ModelState.Clear();

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

            await _eventModelFactory.InsertGuestListAsync(model.eventGuestList);

            return RedirectToAction("EventSetupSummary", new { id = model.UserEventId });
        }

        [HttpGet]
        public async Task<IActionResult> EventSetupSummary(int id)
        {

            return View();
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
