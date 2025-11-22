using AutoMapper;
using SetTheDate.Libraries.Dtos;
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

        public async Task<List<EventImageAttachmentModel>> GetImagesByWeddingCardIdAsync(int weddingCardId)
        {
            var entity = await _AttachmentService.GetImagesByEventWeddingCardIdAsync(weddingCardId);
            return _mapper.Map<List<EventImageAttachmentModel>>(entity);
        }

        public void InsertImageAttachment(List<EventImageAttachmentModel> images, int weddingCardId)
        {
            foreach (var imageModel in images)
            {
                var image = _mapper.Map<EventImageAttachment>(imageModel);
                image.WeddingCardId = weddingCardId;
                _AttachmentService.InsertImage(image);
            }
        }
    }
}
