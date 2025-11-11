using Microsoft.EntityFrameworkCore;
using Repository;
using SetTheDate.Libraries.Dtos;

namespace SetTheDate.Libraries.Repositories
{
    public class EventImageAttachmentRepository : Repository<EventImageAttachment, int>
    {
        public EventImageAttachmentRepository(DbContext context) : base(context)
        {
        }
    }
}
