using Microsoft.EntityFrameworkCore;
using Repository;
using SetTheDate.Libraries.Dtos;

namespace SetTheDate.Libraries.Repositories
{
    public class UserRepository : Repository<User, int>
    {
        public UserRepository(DbContext context) : base(context)
        {
        }
    }
}
