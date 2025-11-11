using AutoMapper;
using SetTheDate.Libraries.Services;
using SetTheDate.Models;

namespace SetTheDate.ModelFactories
{
    public class AttachmentModelFactory
    {
        private readonly AttachmentService _AttachmentService;
        private readonly IMapper _mapper;

        public AttachmentModelFactory(AttachmentService AttachmentService,
            IMapper mapper)
        {
            _AttachmentService = AttachmentService;
            _mapper = mapper;
        }

        public async Task<EventImageAttachmentModel> GetEntityByIdAsync(int id)
        {
            var entity = await _AttachmentService.GetEntityById(id);
            var model = _mapper.Map<EventImageAttachmentModel>(entity);
            return model;
        }

        public async Task<IEnumerable<EventImageAttachmentModel>> GetAllEntitiesAsync()
        {
            var entities = await _AttachmentService.GetAllEntities();
            var models = _mapper.Map<List<EventImageAttachmentModel>>(entities);
            return models;
        }
    }
}
