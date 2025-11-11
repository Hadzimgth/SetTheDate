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

        public async Task<WeddingCardInformation> GetEntityById(int id)
        {
            return await _weddingCardInformationRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<WeddingCardInformation>> GetAllEntities()
        {
            return await _weddingCardInformationRepository.GetAllAsync();
        }
    }
}
