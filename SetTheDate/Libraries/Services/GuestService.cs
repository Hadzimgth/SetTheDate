using Microsoft.EntityFrameworkCore;
using SetTheDate.Libraries.Dtos;
using SetTheDate.Libraries.Repositories;

namespace SetTheDate.Libraries.Services
{
    public class GuestService
    {
        public readonly WeddingCardInformationRepository _weddingCardInformationRepository;
        public readonly GuestWishesRepository _guestWishesRepository;
        public readonly ContactInformationRepository _contactInformationRepository;
        public readonly EventGuestAnswerRepository _eventGuestAnswerRepository;
        private readonly ApplicationDbContext _context;
        private readonly EventService _eventService;

        public GuestService(WeddingCardInformationRepository weddingCardInformationRepository,
            GuestWishesRepository guestWishesRepository,
            ContactInformationRepository contactInformationRepository,
            EventGuestAnswerRepository eventGuestAnswerRepository,
            ApplicationDbContext context,
            EventService eventservice)
        {
            _weddingCardInformationRepository = weddingCardInformationRepository;
            _guestWishesRepository = guestWishesRepository;
            _contactInformationRepository = contactInformationRepository;
            _eventGuestAnswerRepository = eventGuestAnswerRepository;
            _context = context;
            _eventService = eventservice;
        }

        public async Task<WeddingCardInformation> GetWeddingCardByEventIdAsync(int eventId)
        {
            var weddingCards = await _weddingCardInformationRepository.GetAllAsync();
            return weddingCards.FirstOrDefault(x => x.UserEventId == eventId);
        }
        public async Task InsertImage(WeddingCardInformation weddingCardInfo)
        {
            _weddingCardInformationRepository.Add(weddingCardInfo);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateEvent(WeddingCardInformation weddingCardInfo)
        {
            _weddingCardInformationRepository.Update(weddingCardInfo);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteEvent(WeddingCardInformation weddingCardInfo)
        {
            _weddingCardInformationRepository.Delete(weddingCardInfo);
            await _context.SaveChangesAsync();
        }
        public async Task<List<GuestWishes>> GetGuestWishesListByWeddingCardInformationId(int weddingCardInformationId)
        {
            var wishesList = await _guestWishesRepository.GetAllAsync();
            return wishesList.Where(x => x.WeddingCardInformationId == weddingCardInformationId).ToList();
        }
        public async Task InsertWishes(GuestWishes wishes)
        {
            _guestWishesRepository.Add(wishes);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateWishes(GuestWishes wishes)
        {
            _guestWishesRepository.Update(wishes);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteWishes(GuestWishes wishes)
        {
            _guestWishesRepository.Delete(wishes);
            await _context.SaveChangesAsync();
        }

        public async Task<List<ContactInformation>> GetContactInformationListByWeddingCardInformationId(int weddingCardInformationId)
        {
            var contactInformation = await _contactInformationRepository.GetAllAsync();
            return contactInformation.Where(x => x.WeddingCardInformationId == weddingCardInformationId).ToList();
        }
        public async Task InsertContactInformation(ContactInformation contact)
        {
            _contactInformationRepository.Add(contact);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateContactInformation(ContactInformation contact)
        {
            _contactInformationRepository.Update(contact);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteContactInformation(ContactInformation contact)
        {
            _contactInformationRepository.Delete(contact);
            await _context.SaveChangesAsync();
        }

        public async Task<List<EventGuestAnswer>> GetEventGuestAnswerByEventId(int eventId)
        {
            var eventGuestsAnswers = await _eventGuestAnswerRepository.GetAllAsync();
            return eventGuestsAnswers.Where(x => x.UserEventId == eventId).ToList();
        }
        public async Task<EventGuestAnswer> GetGuestanswerByGuestIdAndQuestionId(int guestId, int questionId)
        {
            var guestAnswers = await _eventGuestAnswerRepository.GetAllAsync();
            return guestAnswers.Where(x => x.EventGuestId == guestId && x.EventQuestionId == questionId).FirstOrDefault();
        }
        public async Task<EventGuestAnswer> GetGuestanswersByGuestMobileNumber(string MobileNumber)
        {
            var guestId = (await _eventService.GetGuestByPhoneNumber(MobileNumber)).Id;

            var guestAnswers = await _eventGuestAnswerRepository.GetAllAsync();
            return guestAnswers.Where(x => x.EventGuestId == guestId && x.EventAnswerId == 0).FirstOrDefault();
        }
        public async Task<bool> TryInsertGuestAnswer(EventGuestAnswer answer)
        {
            try
            {
                await InsertGuestAnswer(answer);
                return true;
            }
            catch (DbUpdateException)
            {
                return false;
            }
        }
        public async Task InsertGuestAnswer(EventGuestAnswer guestAnswer)
        {
            _eventGuestAnswerRepository.Add(guestAnswer);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateGuestAnswer(EventGuestAnswer guestAnswer)
        {
            _eventGuestAnswerRepository.Update(guestAnswer);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteGuestAnswer(EventGuestAnswer guestAnswer)
        {
            _eventGuestAnswerRepository.Delete(guestAnswer);
            await _context.SaveChangesAsync();
        }
    }
}
