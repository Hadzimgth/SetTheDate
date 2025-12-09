using Microsoft.EntityFrameworkCore;
using SetTheDate.Libraries.Dtos;
using SetTheDate.Libraries.Repositories;

namespace SetTheDate.Libraries.Services
{
    public class AttachmentService
    {
        public readonly EventImageAttachmentRepository _eventImageAttachmentRepository;
        private readonly ApplicationDbContext _context;

        public AttachmentService(EventImageAttachmentRepository eventImageAttachmentRepository, ApplicationDbContext context)
        {
            _eventImageAttachmentRepository = eventImageAttachmentRepository;
            _context = context;
        }

        public async Task<List<EventImageAttachment>> GetImagesByEventWeddingCardIdAsync(int weddingCardId)
        {
            var images = await _eventImageAttachmentRepository.GetAllAsync();

            return images.Where(x => x.WeddingCardId == weddingCardId).ToList();
        }
        public async Task InsertImage(EventImageAttachment image)
        {
            _eventImageAttachmentRepository.Add(image);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateImage(EventImageAttachment image)
        {
            _eventImageAttachmentRepository.Update(image);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteImage(EventImageAttachment image)
        {
            _eventImageAttachmentRepository.Delete(image);
            await _context.SaveChangesAsync();
        }
    }
}
