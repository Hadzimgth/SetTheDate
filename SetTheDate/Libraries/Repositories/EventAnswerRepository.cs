using Repository;
using SetTheDate.Libraries.Dtos;

namespace SetTheDate.Libraries.Repositories
{
    public class EventAnswerRepository : Repository<EventAnswer, int>
    {
        public EventAnswerRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
