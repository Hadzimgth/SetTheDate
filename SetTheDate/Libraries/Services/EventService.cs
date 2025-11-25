using Microsoft.EntityFrameworkCore;
using SetTheDate.Libraries.Dtos;
using SetTheDate.Libraries.Repositories;

namespace SetTheDate.Libraries.Services
{
    public class EventService
    {
        public readonly UserEventRepository _userEventRepository;
        public readonly EventQuestionRepository _eventQuestionRepository;
        public readonly EventGuestRepository _eventGuestRepository;
        public readonly EventGuestAnswerRepository _eventGuestAnswerRepository;
        public readonly EventAnswerRepository _eventAnswerRepository;
        public readonly WeddingCardInformationRepository _weddingCardInformationRepository;

        public EventService(UserEventRepository userEventRepository,
            EventQuestionRepository eventQuestionRepository,
            EventGuestRepository eventGuestRepository,
            EventAnswerRepository eventAnswerRepository,
            WeddingCardInformationRepository weddingCardInformationRepository)
        {
            _userEventRepository = userEventRepository;
            _eventQuestionRepository = eventQuestionRepository;
            _eventGuestRepository = eventGuestRepository;
            _eventAnswerRepository = eventAnswerRepository;
            _weddingCardInformationRepository = weddingCardInformationRepository;
        }

        public async Task<UserEvent> GetEventByIdAsync(int id)
        {
            return await _userEventRepository.GetByIdAsync(id);
        }

        public async Task<List<UserEvent>> GetEventListAsync(int userId)
        {
            var eventList = (await _userEventRepository.GetAllAsync()).Where(x => x.UserId == userId).ToList();

            return eventList;
        }
        public async Task<UserEvent> InsertEvent(UserEvent userEvent)
        {
            _userEventRepository.Add(userEvent);

            //await _dbContext.SaveChangesAsync();

            return userEvent;
        }
        public async Task<UserEvent> UpdateEvent(UserEvent userEvent)
        {
            _userEventRepository.Update(userEvent);

            //await _dbContext.SaveChangesAsync();

            return userEvent;
        }
        public void DeleteEvent(UserEvent userEvent)
        {
            _userEventRepository.Delete(userEvent);
        }


        public async Task<WeddingCardInformation> InsertWeddingCardInformation(WeddingCardInformation userWeddingCard)
        {
            _weddingCardInformationRepository.Add(userWeddingCard);

            //await _dbContext.SaveChangesAsync();

            return userWeddingCard;
        }
        public async Task<WeddingCardInformation> UpdateWeddingCardInformation(WeddingCardInformation userWeddingCard)
        {
            _weddingCardInformationRepository.Update(userWeddingCard);

            //await _dbContext.SaveChangesAsync();

            return userWeddingCard;
        }
        public void DeleteWeddingCardInformation(WeddingCardInformation userWeddingCard)
        {
            _weddingCardInformationRepository.Delete(userWeddingCard);
        }

        public async Task<List<EventGuest>> GetEventGuestListByIEventdAsync(int eventId)
        {
            var eventGuestList = (await _eventGuestRepository.GetAllAsync()).Where(x => x.UserEventId == eventId).ToList();

            return eventGuestList;
        }
        public void InsertEventGuestListById(List<EventGuest> eventGuests)
        {
            foreach (var guest in eventGuests)
            {
                _eventGuestRepository.Add(guest);
            }
        }
        public void UpdateEventGuestListById(List<EventGuest> eventGuests)
        {
            foreach (var guest in eventGuests)
            {
                _eventGuestRepository.Update(guest);
            }
        }
        public void DeleteEventGuestListById(List<EventGuest> eventGuests)
        {
            foreach (var guest in eventGuests)
            {
                _eventGuestRepository.Delete(guest);
            }
        }

        public async Task<List<EventQuestion>> GetEventQuestionListByIdAsync(int eventId)
        {
            var eventQuestionList = (await _eventQuestionRepository.GetAllAsync()).Where(x => x.UserEventId == eventId).ToList();

            return eventQuestionList;
        }
        public void InsertEventQuestionById(EventQuestion eventQuestion)
        {
            _eventQuestionRepository.Add(eventQuestion);
        }
        public void UpdateEventQuestionListById(List<EventQuestion> eventQuestions)
        {
            foreach (var guest in eventQuestions)
            {
                _eventQuestionRepository.Update(guest);
            }
        }
        public void DeleteEventQuestionListById(List<EventQuestion> eventQuestions)
        {
            foreach (var guest in eventQuestions)
            {
                _eventQuestionRepository.Delete(guest);
            }
        }
        public async Task<List<EventAnswer>> GetEventAnswerListByEventIdAsync(int EventId)
        {
            var eventAnswerList = (await _eventAnswerRepository.GetAllAsync()).Where(x => x.EventId == EventId).ToList();

            return eventAnswerList;
        }
        public void InsertEventAnswer(EventAnswer eventAnswer)
        {
            _eventAnswerRepository.Add(eventAnswer);
        }
        public void UpdateEventAnswer(EventAnswer eventAnswer)
        {
            _eventAnswerRepository.Update(eventAnswer);
        }
        public void DeleteEventAnswer(EventAnswer eventAnswer)
        {
            _eventAnswerRepository.Delete(eventAnswer);
        }
        public async Task<List<EventGuestAnswer>> GetGuestAnswerListByEventIdAsync(int eventId)
        {
            var eventAnswerList = (await _eventGuestAnswerRepository.GetAllAsync()).Where(x => x.EventId == eventId).ToList();

            return eventAnswerList;
        }
        public void InsertGuestAnswer(EventGuestAnswer answer)
        {
            _eventGuestAnswerRepository.Add(answer);
        }
        public void DeleteGeestAnswer(EventGuestAnswer answer)
        {
            _eventGuestAnswerRepository.Delete(answer);
        }
    }
}
