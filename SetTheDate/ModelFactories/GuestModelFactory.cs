using AutoMapper;
using SetTheDate.Libraries.Services;
using SetTheDate.Models;

namespace SetTheDate.ModelFactories
{
    public class GuestModelFactory
    {
        private readonly GuestService _guestService;
        private readonly IMapper _mapper;

        public GuestModelFactory(GuestService guestService,
            IMapper mapper)
        {
            _guestService = guestService;
            _mapper = mapper;
        }

        public async Task<WeddingCardInformationModel> GetEntityByIdAsync(int id)
        {
            var entity = await _guestService.GetEntityById(id);
            var model = _mapper.Map<WeddingCardInformationModel>(entity);
            return model;
        }

        public async Task<IEnumerable<WeddingCardInformationModel>> GetAllEntitiesAsync()
        {
            var entities = await _guestService.GetAllEntities();
            var models = _mapper.Map<List<WeddingCardInformationModel>>(entities);
            return models;
        }
    }
}
