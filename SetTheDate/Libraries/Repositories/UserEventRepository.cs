using Repository;
using SetTheDate.Libraries.Dtos;

namespace SetTheDate.Libraries.Repositories
{
    public class UserEventRepository : Repository<UserEvent, int>
    {
        public UserEventRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
