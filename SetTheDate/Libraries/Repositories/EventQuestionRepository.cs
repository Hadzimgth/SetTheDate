using Microsoft.EntityFrameworkCore;
using Repository;
using SetTheDate.Libraries.Dtos;

namespace SetTheDate.Libraries.Repositories
{
    public class EventQuestionRepository : Repository<EventQuestion, int>
    {
        public EventQuestionRepository(DbContext context) : base(context)
        {
        }
    }
}
