using ExcelDataReader;
using FluentValidation;
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
        private readonly IValidator<UserEventModel> _userEventModelValidator;
        private readonly IValidator<EventSurveySetup> _eventSurveySetupValidator;

        public EventController(EventModelFactory eventModelFactory, IValidator<UserEventModel> userEventModelValidator, IValidator<EventSurveySetup> eventSurveySetupValidator)
        {
            _eventModelFactory = eventModelFactory;
            _userEventModelValidator = userEventModelValidator;
            _eventSurveySetupValidator = eventSurveySetupValidator;
        }

        [HttpGet]
        public async Task<IActionResult> EventSetup(int? eventId)
        {
            int userId = HttpContext.Session.GetInt32("UserId") ?? 0;

            var userEvent = new UserEventModel
            {
                UserId = userId,
                EventDate = DateTime.Now.AddMonths(3),
                EndDate = DateTime.Now.AddMonths(4),
                Status = "Draft"
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
            var validationResult = await _userEventModelValidator.ValidateAsync(userEvent);

            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }
                userEvent.IsEdit = false;
                return View("EventSetup", userEvent);
            }

            var userEventModel = await _eventModelFactory.InsertUserEventAsync(userEvent);
                        
            return RedirectToAction("EventWeddingCardTemplate", new { id = userEventModel.Id });
        }

        [HttpPost]
        public async Task<IActionResult> Edit(UserEventModel userEvent)
        {
            // Check if event status is Completed - skip update if so
            var existingEvent = await _eventModelFactory.GetEventByIdAsync(userEvent.Id);
            if (existingEvent?.Status == "Completed")
            {
                return RedirectToAction("EventWeddingCardTemplate", new { id = userEvent.Id });
            }

            var validationResult = await _userEventModelValidator.ValidateAsync(userEvent);

            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }
                return View("EventSetup", userEvent);
            }

            var userEventModel = await _eventModelFactory.UpdateUserEventAsync(userEvent);
            return RedirectToAction("EventWeddingCardTemplate", new { id = userEventModel.Id });
        }

        [HttpGet]
        public async Task<IActionResult> EventWeddingCardTemplate(int? id)
        {
            var userEvent = await _eventModelFactory.GetEventAndWeddingCardByIdAsync(id.Value);
            return View(userEvent);
        }

        [HttpPost]
        public async Task<IActionResult> WeddingCardTemplateEdit(UserEventModel userEvent)
        {
            var weddingCard = await _eventModelFactory.GetWeddingCardByIdAsync(userEvent.WeddingCardId);
            
            // Check if event status is Completed - skip update if so
            var existingEvent = await _eventModelFactory.GetEventByIdAsync(weddingCard.UserEventId);
            if (existingEvent?.Status == "Completed")
            {
                return RedirectToAction("GuestQuestion", new { id = weddingCard.UserEventId });
            }

            weddingCard.WeddingCardType = userEvent.WeddingCardType;

            await _eventModelFactory.UpdateWeddingCardAsync(weddingCard);

            return RedirectToAction("GuestQuestion", new { id = weddingCard.UserEventId });
        }

        [HttpGet]
        public async Task<IActionResult> GuestQuestion(int id)
        {
            var eventModel = await _eventModelFactory.GetEventAndWeddingCardByIdAsync(id);

            var eventSurvey = new EventSurveySetup();
            eventSurvey.UserEventId = id;

            var existingQuestions = await _eventModelFactory.GetAllEventQuestionListByEventIdAsync(id);

            if (existingQuestions != null && existingQuestions.Any())
            {
                eventSurvey.EventQuestionListModel = existingQuestions;
                eventSurvey.IsEdit = true;
            }
            else
            {
                var answerList = new List<EventAnswerModel>();
                var answer = new EventAnswerModel();
                answer.Answer = "1. Yes";
                answerList.Add(answer);
                answer = new EventAnswerModel();
                answer.Answer = "2. No";
                answerList.Add(answer);

                var eventQuestionModel = new EventQuestionModel();
                eventQuestionModel.Question = $"you are invited to {eventModel.BrideName} & {eventModel.GroomName} on {eventModel.EventDate}. Would you be able to participate? \nPlease answer based on the numbers only.";
                eventQuestionModel.EventAnswerListModel = answerList;

                eventSurvey.EventQuestionListModel.Add(eventQuestionModel);
                eventSurvey.IsEdit = false;
            }

            return View(eventSurvey);
        }

        [HttpPost]
        public async Task<IActionResult> GuestQuestionCreate(EventSurveySetup surveySetup)
        {

            if (surveySetup == null)
            {
                surveySetup = new EventSurveySetup();
            }
            if (surveySetup.EventQuestionListModel == null)
            {
                surveySetup.EventQuestionListModel = new List<EventQuestionModel>();
            }
            

            surveySetup.EventQuestionListModel = surveySetup.EventQuestionListModel
                .Where(q => q != null)
                .ToList();
                
            foreach (var question in surveySetup.EventQuestionListModel)
            {
                if (question.EventAnswerListModel == null)
                {
                    question.EventAnswerListModel = new List<EventAnswerModel>();
                }

                question.EventAnswerListModel = question.EventAnswerListModel
                    .Where(a => a != null)
                    .ToList();
            }
            
            surveySetup.EventQuestionListModel = surveySetup.EventQuestionListModel
                .Where(q => !string.IsNullOrWhiteSpace(q.Question) || (q.EventAnswerListModel != null && q.EventAnswerListModel.Any(a => !string.IsNullOrWhiteSpace(a.Answer))))
                .ToList();
            
            foreach (var question in surveySetup.EventQuestionListModel)
            {
                question.EventAnswerListModel = question.EventAnswerListModel
                    .Where(a => !string.IsNullOrWhiteSpace(a.Answer))
                    .ToList();
            }

            var validationResult = await _eventSurveySetupValidator.ValidateAsync(surveySetup);

            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }
                
                surveySetup.IsEdit = false;
                return View("GuestQuestion", surveySetup);
            }

            await _eventModelFactory.InsertEventQuestionListAsync(surveySetup);

            return RedirectToAction("GuestSetup", new { id = surveySetup.UserEventId });
        }

        [HttpPost]
        public async Task<IActionResult> GuestQuestionEdit(EventSurveySetup surveySetup)
        {
            // Check if event status is Completed - skip update if so
            if (surveySetup != null && surveySetup.UserEventId > 0)
            {
                var existingEvent = await _eventModelFactory.GetEventByIdAsync(surveySetup.UserEventId);
                if (existingEvent?.Status == "Completed")
                {
                    return RedirectToAction("GuestSetup", new { id = surveySetup.UserEventId });
                }
            }

            if (surveySetup == null)
            {
                surveySetup = new EventSurveySetup();
            }
            if (surveySetup.EventQuestionListModel == null)
            {
                surveySetup.EventQuestionListModel = new List<EventQuestionModel>();
            }
            
            surveySetup.EventQuestionListModel = surveySetup.EventQuestionListModel
                .Where(q => q != null)
                .ToList();
                
            foreach (var question in surveySetup.EventQuestionListModel)
            {
                if (question.EventAnswerListModel == null)
                {
                    question.EventAnswerListModel = new List<EventAnswerModel>();
                }

                question.EventAnswerListModel = question.EventAnswerListModel
                    .Where(a => a != null)
                    .ToList();
            }
            
            surveySetup.EventQuestionListModel = surveySetup.EventQuestionListModel
                .Where(q => !string.IsNullOrWhiteSpace(q.Question) || (q.EventAnswerListModel != null && q.EventAnswerListModel.Any(a => !string.IsNullOrWhiteSpace(a.Answer))))
                .ToList();
            
            foreach (var question in surveySetup.EventQuestionListModel)
            {
                question.EventAnswerListModel = question.EventAnswerListModel
                    .Where(a => !string.IsNullOrWhiteSpace(a.Answer))
                    .ToList();
            }

            // Edit and Create do the same thing since we delete and recreate all questions/answers
            var validationResult = await _eventSurveySetupValidator.ValidateAsync(surveySetup);

            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }
                
                surveySetup.IsEdit = true;
                return View("GuestQuestion", surveySetup);
            }

            await _eventModelFactory.InsertEventQuestionListAsync(surveySetup);

            return RedirectToAction("GuestSetup", new { id = surveySetup.UserEventId });
        }

        [HttpGet]
        public async Task<IActionResult> GuestSetup(int id)
        {
            var guestSetup = new EventGuestListModel();
            guestSetup.UserEventId = id;

            // Load existing guests if any
            var existingGuests = await _eventModelFactory.GetAllEventGuestListByEventIdAsync(id);
            if (existingGuests != null && existingGuests.Any())
            {
                guestSetup.eventGuestList = existingGuests;
            }

            return View(guestSetup);
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
            // Check if event status is Completed - skip update if so
            if (model != null && model.UserEventId > 0)
            {
                var existingEvent = await _eventModelFactory.GetEventByIdAsync(model.UserEventId);
                if (existingEvent?.Status == "Completed")
                {
                    return RedirectToAction("EventSetupSummary", new { id = model.UserEventId });
                }
            }

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
            var userEvent = await _eventModelFactory.GetEventByIdAsync(id);
            var questionList = await _eventModelFactory.GetAllEventQuestionListByEventIdAsync(id);

            ViewBag.TotalGuest = userEvent?.TotalGuest ?? 0;
            ViewBag.QuestionCount = questionList?.Count ?? 0;
            ViewBag.EventId = id;
            ViewBag.EventStatus = userEvent?.Status ?? "Draft";

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> MakePayment(int id)
        {
            await _eventModelFactory.MarkEventAsOngoingAsync(id);
            return RedirectToAction("Dashboard", "Home");
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

        [HttpGet]
        public async Task<IActionResult> EventSummary(int id)
        {
            var summary = await _eventModelFactory.GetEventSummaryAsync(id);
            return View(summary);
        }
    }
}
