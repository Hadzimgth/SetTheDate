using AutoMapper;
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

        public async Task<UserEventModel> GetEntityByIdAsync(int id)
        {
            var entity = await _eventService.GetEntityById(id);
            var model = _mapper.Map<UserEventModel>(entity);
            return model;
        }

        public async Task<IEnumerable<UserEventModel>> GetAllEntitiesAsync()
        {
            var entities = await _eventService.GetAllEntities();
            var models = _mapper.Map<List<UserEventModel>>(entities);
            return models;
        }
    }
}
