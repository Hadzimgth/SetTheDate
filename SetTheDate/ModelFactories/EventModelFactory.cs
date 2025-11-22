using AutoMapper;
using SetTheDate.Libraries.Dtos;
using SetTheDate.Libraries.Services;
using SetTheDate.Models;

namespace SetTheDate.ModelFactories
{
    public class EventModelFactory
    {
        private readonly EventService _eventService;
        private readonly IMapper _mapper;

        public EventModelFactory(EventService eventService,
            IMapper mapper)
        {
            _eventService = eventService;
            _mapper = mapper;
        }

        public async Task<UserEventModel> GetEventByIdAsync(int id)
        {
            var entity = await _eventService.GetEventByIdAsync(id);
            var model = _mapper.Map<UserEventModel>(entity);
            return model;
        }
        public async Task<List<UserEventModel>> GetAllEventByUserIdAsync(int userId)
        {
            var eventList = await _eventService.GetEventListAsync(userId);
            var modelList = _mapper.Map<List<UserEventModel>>(eventList);
            return modelList;
        }

        public async Task<UserEventModel> InsertUserEventAsync(UserEventModel userEventModel)
        {
            var entity = _mapper.Map<UserEvent>(userEventModel);
            _eventService.InsertEvent(entity);

            //double confirm if this returns the id or not
            var model = _mapper.Map<UserEventModel>(entity);
            return model;
        }
        public async Task<UserEventModel> UpdateUserEventAsync(UserEventModel userEventModel)
        {
            var entity = _mapper.Map<UserEvent>(userEventModel);
            _eventService.UpdateEvent(entity);

            //double confirm if this returns the id or not
            var model = _mapper.Map<UserEventModel>(entity);
            return model;
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
        public void InsertGuestListAsync(List<EventGuestModel> eventGuestList, int eventId)
        {
            var entities = _mapper.Map<List<EventGuest>>(eventGuestList);
            _eventService.InsertEventGuestListById(entities, eventId);

        }
        public void UpdateGuestListAsync(List<EventGuestModel> eventGuestList)
        {
            var entities = _mapper.Map<List<EventGuest>>(eventGuestList);
            _eventService.UpdateEventGuestListById(entities);

        }
        public void DeleteGuestListAsync(List<EventGuestModel> eventGuestList)
        {
            var entities = _mapper.Map<List<EventGuest>>(eventGuestList);
            _eventService.DeleteEventGuestListById(entities);

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
                question.eventAnswerModels = answers;
            }

            return modelList;
        }
        public async Task InsertEventQuestionListAsync(List<EventQuestionModel> eventQuestionModels, int eventId)
        {
            //var entities = _mapper.Map<List<EventQuestion>>(eventQuestionModels);
            foreach (var question in eventQuestionModels)
            {
                var entity = _mapper.Map<EventQuestion>(eventQuestionModels);
                entity.UserEventId = eventId;
                //double confirm if this returns the id or not
                _eventService.InsertEventQuestionById(entity);

                foreach (var answerModel in question.eventAnswerModels)
                {
                    var answerEntity = _mapper.Map<EventAnswer>(answerModel);
                    answerEntity.EventQuestionId = entity.Id;
                    _eventService.InsertEventAnswer(answerEntity);
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
