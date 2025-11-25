using Repository;
using SetTheDate.Libraries.Dtos;

namespace SetTheDate.Libraries.Repositories
{
    public class EventQuestionRepository : Repository<EventQuestion, int>
    {
        public EventQuestionRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
