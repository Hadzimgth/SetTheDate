using SetTheDate.Libraries.Dtos;
using SetTheDate.Libraries.Repositories;

namespace SetTheDate.Libraries.Services
{
    public class AttachmentService
    {
        public readonly EventImageAttachmentRepository _eventImageAttachmentRepository;

        public AttachmentService(EventImageAttachmentRepository eventImageAttachmentRepository)
        {
            _eventImageAttachmentRepository = eventImageAttachmentRepository;
        }

        public async Task<List<EventImageAttachment>> GetImagesByEventWeddingCardIdAsync(int weddingCardId)
        {
            var images = await _eventImageAttachmentRepository.GetAllAsync();

            return images.Where(x => x.WeddingCardId == weddingCardId).ToList();
        }
        public void InsertImage(EventImageAttachment image)
        {
            _eventImageAttachmentRepository.Add(image);
        }
        public void UpdateImage(EventImageAttachment image)
        {
            _eventImageAttachmentRepository.Update(image);
        }
        public void DeleteImage(EventImageAttachment image)
        {
            _eventImageAttachmentRepository.Delete(image);
        }
    }
}
