using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SetTheDate.Libraries.Dtos;
using SetTheDate.Libraries.Repositories;
using SetTheDate.Models;

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
        public readonly ContactInformationRepository _contactInformationRepository;
        private readonly ApplicationDbContext _context;

        public EventService(UserEventRepository userEventRepository,
            EventQuestionRepository eventQuestionRepository,
            EventGuestRepository eventGuestRepository,
            EventAnswerRepository eventAnswerRepository,
            WeddingCardInformationRepository weddingCardInformationRepository,
            ContactInformationRepository contactInformationRepository,
            EventGuestAnswerRepository eventGuestAnswerRepository,
            ApplicationDbContext context)
        {
            _userEventRepository = userEventRepository;
            _eventQuestionRepository = eventQuestionRepository;
            _eventGuestRepository = eventGuestRepository;
            _eventAnswerRepository = eventAnswerRepository;
            _weddingCardInformationRepository = weddingCardInformationRepository;
            _contactInformationRepository = contactInformationRepository;
            _eventGuestAnswerRepository = eventGuestAnswerRepository;
            _context = context;
        }

        public async Task<UserEvent> GetEventByIdAsync(int id)
        {
            return await _userEventRepository.GetByIdAsync(id);
        }

        public async Task<List<UserEvent>> GetAllEventListAsync()
        {
            var eventList = (await _userEventRepository.GetAllAsync()).ToList();

            return eventList;
        }
        public async Task<List<UserEvent>> GetEventListByUserIdAsync(int userId)
        {
            var eventList = (await _userEventRepository.GetAllAsync()).Where(x => x.UserId == userId).ToList();

            return eventList;
        }
        public async Task<UserEvent> InsertEvent(UserEvent userEvent)
        {
            _userEventRepository.Add(userEvent);
            await _context.SaveChangesAsync();

            return userEvent;
        }
        public async Task<UserEvent> UpdateEvent(UserEvent userEvent)
        {
            _userEventRepository.Update(userEvent);
            await _context.SaveChangesAsync();

            return userEvent;
        }
        public async Task UpdateStatusAsync(int id, string status)
        {
            var entity = await GetEventByIdAsync(id);

            if (entity == null) return;

            entity.Status = status;

            _userEventRepository.Update(entity);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteEvent(UserEvent userEvent)
        {
            _userEventRepository.Delete(userEvent);
            await _context.SaveChangesAsync();
        }

        public async Task<WeddingCardInformation> GetWeddingCardByEventIdAsync(int id)
        {
            return (await _weddingCardInformationRepository.GetAllAsync()).Where(x => x.UserEventId == id).FirstOrDefault();
        }
        public async Task<WeddingCardInformation> GetWeddingCardByIdAsync(int id)
        {
            return (await _weddingCardInformationRepository.GetAllAsync()).Where(x => x.Id == id).FirstOrDefault();
        }
        public async Task<WeddingCardInformation> InsertWeddingCardInformation(WeddingCardInformation userWeddingCard)
        {
            _weddingCardInformationRepository.Add(userWeddingCard);
            await _context.SaveChangesAsync();

            return userWeddingCard;
        }
        public async Task<WeddingCardInformation> UpdateWeddingCardInformation(WeddingCardInformation userWeddingCard)
        {
            _weddingCardInformationRepository.Update(userWeddingCard);
            await _context.SaveChangesAsync();

            return userWeddingCard;
        }
        public async Task DeleteWeddingCardInformation(WeddingCardInformation userWeddingCard)
        {
            _weddingCardInformationRepository.Delete(userWeddingCard);
            await _context.SaveChangesAsync();
        }
        public async Task<EventGuest> GetGuestByPhoneNumber(string phoneNumber)
        {
            var guests = await _eventGuestRepository.GetAllAsync();
            return guests.FirstOrDefault(x => x.PhoneNumber == phoneNumber);
        }
        public async Task<List<EventGuest>> GetEventGuestListByIEventdAsync(int eventId)
        {
            var eventGuestList = (await _eventGuestRepository.GetAllAsync()).Where(x => x.UserEventId == eventId && x.IsValid).ToList();

            return eventGuestList;
        }
        public async Task InsertEventGuestList(List<EventGuest> eventGuests)
        {
            foreach (var guest in eventGuests)
            {
                _eventGuestRepository.Add(guest);
            }
            _context.Database.SetCommandTimeout(120);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateEventGuestList(List<EventGuest> eventGuests)
        {
            foreach (var guest in eventGuests)
            {
                _eventGuestRepository.Update(guest);
            }

            await _context.SaveChangesAsync();
        }
        public async Task UpdateEventGuest(EventGuest guest)
        {
            _eventGuestRepository.Update(guest);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteEventGuestList(List<EventGuest> eventGuests)
        {
            foreach (var guest in eventGuests)
            {
                _eventGuestRepository.Delete(guest);
            }
            await _context.SaveChangesAsync();
        }

        public async Task<List<EventQuestion>> GetEventQuestionListByIdAsync(int eventId)
        {
            var eventQuestionList = (await _eventQuestionRepository.GetAllAsync()).Where(x => x.UserEventId == eventId).ToList();

            return eventQuestionList;
        }
        public async Task InsertEventQuestion(EventQuestion eventQuestion)
        {
            _eventQuestionRepository.Add(eventQuestion); 
            await _context.SaveChangesAsync();
        }
        public async Task UpdateEventQuestionList(List<EventQuestion> eventQuestions)
        {
            foreach (var guest in eventQuestions)
            {
                _eventQuestionRepository.Update(guest);
            }

            await _context.SaveChangesAsync();
        }
        public async Task DeleteEventQuestionList(List<EventQuestion> eventQuestions)
        {
            foreach (var guest in eventQuestions)
            {
                _eventQuestionRepository.Delete(guest);
            }

            await _context.SaveChangesAsync();
        }
        public async Task<List<EventAnswer>> GetEventAnswerListByEventIdAsync(int EventId)
        {
            var eventAnswerList = (await _eventAnswerRepository.GetAllAsync()).Where(x => x.EventId == EventId).ToList();

            return eventAnswerList;
        }
        public async Task InsertEventAnswer(EventAnswer eventAnswer)
        {
            _eventAnswerRepository.Add(eventAnswer);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateEventAnswer(EventAnswer eventAnswer)
        {
            _eventAnswerRepository.Update(eventAnswer);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteEventAnswer(EventAnswer eventAnswer)
        {
            _eventAnswerRepository.Delete(eventAnswer);
            await _context.SaveChangesAsync();
        }
        public async Task<List<EventGuestAnswer>> GetGuestAnswerListByEventIdAsync(int eventId)
        {
            var eventAnswerList = (await _eventGuestAnswerRepository.GetAllAsync()).Where(x => x.UserEventId == eventId).ToList();

            return eventAnswerList;
        }
        public async Task InsertGuestAnswer(EventGuestAnswer answer)
        {
            _eventGuestAnswerRepository.Add(answer);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteGuestAnswer(EventGuestAnswer answer)
        {
            _eventGuestAnswerRepository.Delete(answer); 
            await _context.SaveChangesAsync();
        }

        public async Task InsertContactInformationList(List<ContactInformation> contactInformationList)
        {
            foreach (var contact in contactInformationList)
            {
                _contactInformationRepository.Add(contact);
            }
            await _context.SaveChangesAsync();
        }

        public async Task UpdateContactInformationList(List<ContactInformation> contactInformationList)
        {
            foreach (var contact in contactInformationList)
            {
                _contactInformationRepository.Update(contact);
            }
            await _context.SaveChangesAsync();
        }
    }
}
