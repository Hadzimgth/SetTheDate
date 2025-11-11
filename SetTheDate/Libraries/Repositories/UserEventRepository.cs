using Microsoft.EntityFrameworkCore;
using Repository;
using SetTheDate.Libraries.Dtos;

namespace SetTheDate.Libraries.Repositories
{
    public class UserEventRepository : Repository<UserEvent, int>
    {
        public UserEventRepository(DbContext context) : base(context)
        {
        }
    }
}
