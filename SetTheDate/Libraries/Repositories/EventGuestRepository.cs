using Repository;
using SetTheDate.Libraries.Dtos;

namespace SetTheDate.Libraries.Repositories
{
    public class EventGuestRepository : Repository<EventGuest, int>
    {
        public EventGuestRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
