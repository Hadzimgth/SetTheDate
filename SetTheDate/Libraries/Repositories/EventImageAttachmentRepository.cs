using Repository;
using SetTheDate.Libraries.Dtos;

namespace SetTheDate.Libraries.Repositories
{
    public class EventImageAttachmentRepository : Repository<EventImageAttachment, int>
    {
        public EventImageAttachmentRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
