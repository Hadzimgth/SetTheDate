using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PhoneNumbers;
using Repository;
using SetTheDate.Libraries.Dtos;
using SetTheDate.Libraries.Services;
using SetTheDate.Models;

namespace SetTheDate.ModelFactories
{
    public class EventModelFactory
    {
        private readonly EventService _eventService;
        private readonly PaymentService _paymentService;
        private readonly GuestService _guestService;
        private readonly IMapper _mapper;

        public EventModelFactory(EventService eventService,
            PaymentService paymentService, GuestService guestService,
            IMapper mapper)
        {
            _eventService = eventService;
            _paymentService = paymentService;
            _mapper = mapper;
            _guestService = guestService;
        }

        public async Task<UserEventModel> GetEventAndWeddingCardByIdAsync(int id)
        {
            var entity = await _eventService.GetEventByIdAsync(id);

            var weddingCard = await _eventService.GetWeddingCardByEventIdAsync(id);
            var model = new UserEventModel();

            model.WeddingCardId = weddingCard.Id;
            model.GroomName = weddingCard.GroomName;
            model.BrideName = weddingCard.BrideName;
            model.GroomFatherName = weddingCard.GroomFatherName;
            model.GroomMotherName = weddingCard.GroomMotherName;
            model.BrideFatherName = weddingCard.BrideFatherName;
            model.BrideMotherName = weddingCard.BrideMotherName;
            model.Wishes = weddingCard.Wishes;
            model.LocationName = weddingCard.LocationName;
            model.Address1 = weddingCard.Address1;
            model.Address2 = weddingCard.Address2;
            model.Address3 = weddingCard.Address3;
            model.Postcode = weddingCard.Postcode;
            model.State = weddingCard.State;
            model.Id = entity.Id;
            model.EventName = entity.EventName;
            model.EventDescription = entity.EventDescription;
            model.EventDate = entity.EventDate;
            model.EndDate = entity.EndDate;
            model.UserId = entity.UserId;
            model.Status = entity.Status;
            model.WeddingCardType = weddingCard.WeddingCardType;
            model.IsEdit = true;

            var contactInformation = await _guestService.GetContactInformationListByWeddingCardInformationId(weddingCard.Id);
            model.ContactInformations = _mapper.Map<List<ContactInformationModel>>(contactInformation);

            return model;
        }
        public async Task<WeddingCardInformationModel> GetWeddingCardByIdAsync(int id)
        {
            var weddingCard = await _eventService.GetWeddingCardByIdAsync(id);
            var model = _mapper.Map<WeddingCardInformationModel>(weddingCard);

            var contactInformation = await _guestService.GetContactInformationListByWeddingCardInformationId(weddingCard.Id);
            model.ContactInformations = _mapper.Map<List<ContactInformationModel>>(contactInformation);

            var guestWishes = await _guestService.GetGuestWishesListByWeddingCardInformationId(weddingCard.Id);
            model.GuestWishes = _mapper.Map<List<GuestWishesModel>>(guestWishes);

            return model;
        }
        public async Task<DashboardModel> GetAllEventByUserIdAsync(int userId)
        {
            var eventList = await _eventService.GetEventListByUserIdAsync(userId);
            var modelList = _mapper.Map<List<UserEventModel>>(eventList);

            foreach(var model in modelList)
            {
                model.WeddingCardId = (await _eventService.GetWeddingCardByEventIdAsync(model.Id)).Id;
            }

            var dashboardList = new DashboardModel();
            dashboardList.UserEventModelList = modelList;

            dashboardList.DraftEventCount = modelList.Where(x => x.Status == "Draft").Count();
            dashboardList.ActiveEventCount = modelList.Where(x => x.Status == "Ongoing").Count();
            dashboardList.CompletedEventCount = modelList.Where(x => x.Status == "Completed").Count();


            return dashboardList;
        }

        public async Task<UserEventModel> InsertUserEventAsync(UserEventModel userEventModel)
        {
            var paymentInformation = new PaymentInformation();
            paymentInformation.Amount = 0;
            paymentInformation.Status = "Pending";
            paymentInformation.PaymentDate = DateTime.Now;
            await _paymentService.InsertPayment(paymentInformation);

            var userEvent = new UserEvent();
            userEvent.EventName = userEventModel.EventName;
            userEvent.EventDescription = userEventModel.EventDescription;
            userEvent.EventDate = userEventModel.EventDate;
            userEvent.EndDate = userEventModel.EndDate;
            userEvent.PurgeDate = userEventModel.EventDate.AddDays(20);
            userEvent.UserId = userEventModel.UserId;
            userEvent.Status = "Draft";
            userEvent.PaymentInformationId = paymentInformation.Id;
            await _eventService.InsertEvent(userEvent);

            var weddingInfo = new WeddingCardInformation();
            weddingInfo.UserEventId = userEvent.Id;
            weddingInfo.LocationName = userEventModel.LocationName;
            weddingInfo.BrideName = userEventModel.BrideName;
            weddingInfo.GroomName = userEventModel.GroomName;
            weddingInfo.Address1 = userEventModel.Address1;
            weddingInfo.Address2 = userEventModel.Address2;
            weddingInfo.Address3 = userEventModel.Address3;
            weddingInfo.Postcode = userEventModel.Postcode;
            weddingInfo.State = userEventModel.State;
            weddingInfo.WeddingCardType = userEventModel.WeddingCardType;
            weddingInfo.GroomFatherName = userEventModel.GroomFatherName;
            weddingInfo.GroomMotherName = userEventModel.GroomMotherName;
            weddingInfo.BrideFatherName = userEventModel.BrideFatherName;
            weddingInfo.BrideMotherName = userEventModel.BrideMotherName;
            weddingInfo.TimeFrom = userEventModel.EventDate.Date.AddHours(11);
            weddingInfo.TimeTo = userEventModel.EventDate.Date.AddHours(16);
            weddingInfo.Wishes = userEventModel.Wishes;

            await _eventService.InsertWeddingCardInformation(weddingInfo);

            var contactInfoList = new List<ContactInformation>();

            foreach (var contact in userEventModel.ContactInformations)
            {
                var contactEntity = _mapper.Map<ContactInformation>(contact);
                contactEntity.WeddingCardInformationId = weddingInfo.Id;
                contactInfoList.Add(contactEntity);
            }
            await _eventService.InsertContactInformationList(contactInfoList);

            return _mapper.Map<UserEventModel>(userEvent);
        }
        public async Task<UserEventModel> GetEventByIdAsync(int id)
        {
            var userEvent = await _eventService.GetEventByIdAsync(id);
            return _mapper.Map<UserEventModel>(userEvent);
        }
        public async Task<UserEventModel> UpdateUserEventAsync(UserEventModel model)
        {
            var entity = await _eventService.GetEventByIdAsync(model.Id);
            entity.EventName = model.EventName;
            entity.EventDescription = model.EventDescription;
            entity.EventDate = model.EventDate;
            entity.EndDate = model.EndDate;
            entity.EndDate = model.EndDate;
            entity.PurgeDate = model.EndDate.AddDays(30);
            entity.UserId = model.UserId;
            entity.Status = model.Status;
            entity.PaymentInformationId = entity.Id;
            await _eventService.UpdateEvent(entity);

            var weddingInfo = await _eventService.GetWeddingCardByEventIdAsync(model.Id);
            weddingInfo.UserEventId = entity.Id;
            weddingInfo.LocationName = model.LocationName;
            weddingInfo.BrideName = model.BrideName;
            weddingInfo.GroomName = model.GroomName;
            weddingInfo.Address1 = model.Address1;
            weddingInfo.Address2 = model.Address2;
            weddingInfo.Address3 = model.Address3;
            weddingInfo.Postcode = model.Postcode;
            weddingInfo.State = model.State;
            weddingInfo.WeddingCardType = model.WeddingCardType;
            weddingInfo.GroomFatherName = model.GroomFatherName;
            weddingInfo.GroomMotherName = model.GroomMotherName;
            weddingInfo.BrideFatherName = model.BrideFatherName;
            weddingInfo.BrideMotherName = model.BrideMotherName;
            weddingInfo.TimeFrom = model.EventDate.Date.AddHours(11);
            weddingInfo.TimeFrom = model.EventDate.Date.AddHours(16);
            weddingInfo.Wishes = model.Wishes;

            await _eventService.UpdateWeddingCardInformation(weddingInfo);

            return model;
        }
        public async Task MarkEventAsOngoingAsync(int eventId)
        {
            await _eventService.UpdateStatusAsync(eventId, "Ongoing");
        }

        public async Task<WeddingCardInformationModel> UpdateWeddingCardAsync(WeddingCardInformationModel weddingCard)
        {
            var model = await _eventService.GetWeddingCardByIdAsync(weddingCard.Id);
            model.WeddingCardType = weddingCard.WeddingCardType;
            var entity = await _eventService.UpdateWeddingCardInformation(model);

            return _mapper.Map<WeddingCardInformationModel>(entity);
        }
        public void DeleteUserEventAsync(UserEventModel userEventModel)
        {
            var entity = _mapper.Map<UserEvent>(userEventModel);
            _eventService.DeleteEvent(entity);
        }
        public async Task<List<EventGuestModel>> GetAllEventGuestListByEventIdAsync(int eventId)
        {
            var eventList = await _eventService.GetEventGuestListByIEventdAsync(eventId);
            var modelList = _mapper.Map<List<EventGuestModel>>(eventList);
            return modelList;
        }
        public async Task InsertGuestListAsync(List<EventGuestModel> eventGuestList)
        {
            if (eventGuestList == null || !eventGuestList.Any())
                return;

            var eventId = eventGuestList.First().UserEventId;


            var newGuests = eventGuestList.Where(g => g.Id == 0).ToList();
            var existingGuests = eventGuestList.Where(g => g.Id > 0).ToList();

            if (newGuests.Any())
            {
                var newEntities = _mapper.Map<List<EventGuest>>(newGuests);
                await _eventService.InsertEventGuestList(newEntities);
            }

            if (existingGuests.Any())
            {
                var existingEntities = _mapper.Map<List<EventGuest>>(existingGuests);
                await _eventService.UpdateEventGuestList(existingEntities);
            }

            var validGuests = await _eventService.GetEventGuestListByIEventdAsync(eventId);
            var totalGuestCount = validGuests.Count;

            var userEvent = await _eventService.GetEventByIdAsync(eventId);
            if (userEvent != null)
            {
                userEvent.TotalGuest = totalGuestCount;
                await _eventService.UpdateEvent(userEvent);
            }
        }
        public void UpdateGuestListAsync(List<EventGuestModel> eventGuestList)
        {
            var entities = _mapper.Map<List<EventGuest>>(eventGuestList);
            _eventService.UpdateEventGuestList(entities);

        }
        public void DeleteGuestListAsync(List<EventGuestModel> eventGuestList)
        {
            var entities = _mapper.Map<List<EventGuest>>(eventGuestList);
            _eventService.DeleteEventGuestList(entities);

        }

        public bool ValidateMobile(string mobile)
        {
            if (string.IsNullOrWhiteSpace(mobile))
                return false;

            try
            {
                var phoneUtil = PhoneNumberUtil.GetInstance();

                var number = phoneUtil.Parse(mobile, "MY");

                return phoneUtil.IsValidNumber(number) &&
                       phoneUtil.GetNumberType(number) == PhoneNumberType.MOBILE;
            }
            catch (NumberParseException)
            {
                return false;
            }
        }

        public async Task<List<EventQuestionModel>> GetAllEventQuestionListByEventIdAsync(int eventId)
        {
            var eventList = await _eventService.GetEventQuestionListByIdAsync(eventId);
            var modelList = _mapper.Map<List<EventQuestionModel>>(eventList);

            var answerList = await _eventService.GetEventAnswerListByEventIdAsync(eventId);
            var answerListModel = _mapper.Map<List<EventAnswerModel>>(answerList);

            foreach (var question in modelList)
            {
                var answers = answerListModel.Where(x => x.EventQuestionId == question.Id).ToList();
                question.EventAnswerListModel = answers;
            }

            return modelList;
        }
        public async Task InsertEventQuestionListAsync(EventSurveySetup surveySetup)
        {

            var existingGuestAnswers = await _eventService.GetGuestAnswerListByEventIdAsync(surveySetup.UserEventId);
            foreach (var guestAnswer in existingGuestAnswers)
            {
                await _eventService.DeleteGeestAnswer(guestAnswer);
            }

            var existingAnswers = await _eventService.GetEventAnswerListByEventIdAsync(surveySetup.UserEventId);
            foreach (var answer in existingAnswers)
            {
                await _eventService.DeleteEventAnswer(answer);
            }

            var existingQuestions = await _eventService.GetEventQuestionListByIdAsync(surveySetup.UserEventId);
            foreach (var question in existingQuestions)
            {
                await _eventService.DeleteEventQuestionList(new List<EventQuestion> { question });
            }

            int sequence = 0;
            foreach (var question in surveySetup.EventQuestionListModel)
            {
                var entity = _mapper.Map<EventQuestion>(question);
                entity.UserEventId = surveySetup.UserEventId;
                entity.Sequence = sequence++;
                await _eventService.InsertEventQuestion(entity);

                foreach (var answerModel in question.EventAnswerListModel)
                {
                    var answerEntity = _mapper.Map<EventAnswer>(answerModel);
                    answerEntity.EventQuestionId = entity.Id;
                    answerEntity.EventId = surveySetup.UserEventId;
                    await _eventService.InsertEventAnswer(answerEntity);
                }
            }

        }

        public async Task<EventSummaryModel> GetEventSummaryAsync(int eventId)
        {
            var eventModel = await GetEventByIdAsync(eventId);
            var guests = await GetAllEventGuestListByEventIdAsync(eventId);
            var questions = await GetAllEventQuestionListByEventIdAsync(eventId);
            var guestAnswers = await _eventService.GetGuestAnswerListByEventIdAsync(eventId);
            var eventAnswers = await _eventService.GetEventAnswerListByEventIdAsync(eventId);

            var summary = new EventSummaryModel
            {
                EventId = eventId,
                EventName = eventModel?.EventName ?? ""
            };

            int completeCount = 0;
            int incompleteCount = 0;
            int noResponseCount = 0;

            foreach (var guest in guests)
            {
                var guestResponse = new GuestResponseDetailModel
                {
                    GuestId = guest.Id,
                    GuestName = guest.GuestName,
                    MobileNumber = guest.PhoneNumber
                };


                var answersForGuest = guestAnswers.Where(ga => ga.EventGuestId == guest.Id).ToList();
                
                var answeredQuestions = answersForGuest.Where(a => a.EventAnswerId != 0).ToList();
                var totalQuestions = questions.Count;
                var answeredCount = answeredQuestions.Count;


                if (answeredCount == 0)
                {

                    noResponseCount++;
                    guestResponse.ResponseStatus = "No Response";
                }
                else if (answeredCount == totalQuestions)
                {

                    completeCount++;
                    guestResponse.ResponseStatus = "Complete";
                }
                else
                {
                    incompleteCount++;
                    guestResponse.ResponseStatus = "Incomplete";
                }


                foreach (var question in questions.OrderBy(q => q.Id))
                {
                    var guestAnswer = answersForGuest.FirstOrDefault(ga => ga.EventQuestionId == question.Id);
                    var questionAnswer = new QuestionAnswerModel
                    {
                        QuestionId = question.Id,
                        QuestionText = question.Question
                    };

                    if (guestAnswer != null && guestAnswer.EventAnswerId != 0)
                    {
                        var eventAnswer = eventAnswers.FirstOrDefault(ea => ea.Id == guestAnswer.EventAnswerId);
                        if (eventAnswer != null)
                        {
                            questionAnswer.AnswerText = eventAnswer.Answer;
                            questionAnswer.AnswerKeyword = eventAnswer.AnswerKeyword;
                        }
                    }

                    guestResponse.QuestionAnswers.Add(questionAnswer);
                }

                summary.GuestResponses.Add(guestResponse);
            }

            summary.CompleteCount = completeCount;
            summary.IncompleteCount = incompleteCount;
            summary.NoResponseCount = noResponseCount;

            return summary;
        }
    }
}
