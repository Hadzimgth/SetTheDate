using Microsoft.EntityFrameworkCore;
using Repository;
using SetTheDate.Libraries.Dtos;

namespace SetTheDate.Libraries.Repositories
{
    public class EventGuestAnswerRepository : Repository<EventGuestAnswer, int>
    {
        public EventGuestAnswerRepository(DbContext context) : base(context)
        {
        }
    }
}
