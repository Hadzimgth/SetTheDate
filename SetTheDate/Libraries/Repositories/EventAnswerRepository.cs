using Microsoft.EntityFrameworkCore;
using Repository;
using SetTheDate.Libraries.Dtos;

namespace SetTheDate.Libraries.Repositories
{
    public class EventAnswerRepository : Repository<EventAnswer, int>
    {
        public EventAnswerRepository(DbContext context) : base(context)
        {
        }
    }
}
