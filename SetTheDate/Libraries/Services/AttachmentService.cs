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

        public async Task<EventImageAttachment> GetEntityById(int id)
        {
            return await _eventImageAttachmentRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<EventImageAttachment>> GetAllEntities()
        {
            return await _eventImageAttachmentRepository.GetAllAsync();
        }
    }
}
