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
        private readonly IMapper _mapper;

        public EventModelFactory(EventService eventService,
            PaymentService paymentService,
            IMapper mapper)
        {
            _eventService = eventService;
            _paymentService = paymentService;
            _mapper = mapper;
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

            return model;
        }
        public async Task<WeddingCardInformationModel> GetWeddingCardByIdAsync(int id)
        {
            var weddingCard = await _eventService.GetWeddingCardByIdAsync(id);
            var model = _mapper.Map<WeddingCardInformationModel>(weddingCard);

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
            weddingInfo.TimeFrom = userEventModel.EventDate.Date.AddHours(16);

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
        public async Task<UserEventModel> UpdateUserEventAsync(UserEventModel userEventModel)
        {
            var entity = _mapper.Map<UserEvent>(userEventModel);
            await _eventService.UpdateEvent(entity);

            var model = _mapper.Map<UserEventModel>(entity);
            return model;
        }
        public async Task<WeddingCardInformationModel> UpdateWeddingCardAsync(WeddingCardInformationModel weddingCard)
        {
            var entity = await _eventService.UpdateWeddingCardInformation(weddingCard);

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
            var entities = _mapper.Map<List<EventGuest>>(eventGuestList);
            await _eventService.InsertEventGuestList(entities);

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
        public async Task InsertEventQuestionListAsync(EventSurveySetup surverySetup)
        {
            //var entities = _mapper.Map<List<EventQuestion>>(eventQuestionModels);
            int sequence = 0;
            foreach (var question in surverySetup.EventQuestionListModel)
            {
                var entity = _mapper.Map<EventQuestion>(question);
                entity.UserEventId = surverySetup.UserEventId;
                entity.Sequence = sequence++;
                await _eventService.InsertEventQuestion(entity);

                foreach (var answerModel in question.EventAnswerListModel)
                {
                    var answerEntity = _mapper.Map<EventAnswer>(answerModel);
                    answerEntity.EventQuestionId = entity.Id;
                    answerEntity.EventId = surverySetup.UserEventId;
                    await _eventService.InsertEventAnswer(answerEntity);
                }
            }

        }
        //public void UpdateGuestListAsync(List<EventGuestModel> eventGuestList)
        //{
        //    var entities = _mapper.Map<List<EventGuest>>(eventGuestList);
        //    _eventService.UpdateEventGuestListById(entities);

        //}
        //public void DeleteGuestListAsync(List<EventGuestModel> eventGuestList)
        //{
        //    var entities = _mapper.Map<List<EventGuest>>(eventGuestList);
        //    _eventService.DeleteEventGuestListById(entities);

        //}
    }
}
