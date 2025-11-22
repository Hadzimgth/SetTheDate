using SetTheDate.Libraries.Dtos;
using SetTheDate.Libraries.Repositories;

namespace SetTheDate.Libraries.Services
{
    public class GuestService
    {
        public readonly WeddingCardInformationRepository _weddingCardInformationRepository;
        public readonly GuestWishesRepository _guestWishesRepository;
        public readonly ContactInformationRepository _contactInformationRepository;

        public GuestService(WeddingCardInformationRepository weddingCardInformationRepository)
        {
            _weddingCardInformationRepository = weddingCardInformationRepository;
        }

        public async Task<WeddingCardInformation> GetWeddingCardByEventIdAsync(int eventId)
        {
            var weddingCards = await _weddingCardInformationRepository.GetAllAsync();
            return weddingCards.FirstOrDefault(x => x.UserEventId == eventId);
        }
        public void InsertImage(WeddingCardInformation weddingCardInfo)
        {
            _weddingCardInformationRepository.Add(weddingCardInfo);
        }
        public void UpdateEvent(WeddingCardInformation weddingCardInfo)
        {
            _weddingCardInformationRepository.Update(weddingCardInfo);
        }
        public void DeleteEvent(WeddingCardInformation weddingCardInfo)
        {
            _weddingCardInformationRepository.Delete(weddingCardInfo);
        }
        public async Task<List<GuestWishes>> GetGuestWishesListByWeddingCardInformationId(int weddingCardInformationId)
        {
            var wishesList = await _guestWishesRepository.GetAllAsync();
            return wishesList.Where(x => x.WeddingCardInformationId == weddingCardInformationId).ToList();
        }
        public void InsertWishes(GuestWishes wishes)
        {
            _guestWishesRepository.Add(wishes);
        }
        public void UpdateWishes(GuestWishes wishes)
        {
            _guestWishesRepository.Update(wishes);
        }
        public void DeleteWishes(GuestWishes wishes)
        {
            _guestWishesRepository.Delete(wishes);
        }

        public async Task<List<ContactInformation>> GetContactInformationListByWeddingCardInformationId(int weddingCardInformationId)
        {
            var contactInformation = await _contactInformationRepository.GetAllAsync();
            return contactInformation.Where(x => x.WeddingCardInformationId == weddingCardInformationId).ToList();
        }
        public void InsertContactInformation(ContactInformation contact)
        {
            _contactInformationRepository.Add(contact);
        }
        public void UpdateContactInformation(ContactInformation contact)
        {
            _contactInformationRepository.Update(contact);
        }
        public void DeleteContactInformation(ContactInformation contact)
        {
            _contactInformationRepository.Delete(contact);
        }
    }
}
