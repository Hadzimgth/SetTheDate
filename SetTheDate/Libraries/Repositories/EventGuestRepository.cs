using Microsoft.EntityFrameworkCore;
using Repository;
using SetTheDate.Libraries.Dtos;

namespace SetTheDate.Libraries.Repositories
{
    public class EventGuestRepository : Repository<EventGuest, int>
    {
        public EventGuestRepository(DbContext context) : base(context)
        {
        }
    }
}
