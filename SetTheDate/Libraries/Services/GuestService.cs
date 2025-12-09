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
        private readonly ApplicationDbContext _context;

        public GuestService(WeddingCardInformationRepository weddingCardInformationRepository, ApplicationDbContext context)
        {
            _weddingCardInformationRepository = weddingCardInformationRepository;
            _context = context;
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
    }
}
