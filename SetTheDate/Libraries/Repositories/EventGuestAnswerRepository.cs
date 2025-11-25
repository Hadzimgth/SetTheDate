using Repository;
using SetTheDate.Libraries.Dtos;

namespace SetTheDate.Libraries.Repositories
{
    public class EventGuestAnswerRepository : Repository<EventGuestAnswer, int>
    {
        public EventGuestAnswerRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
