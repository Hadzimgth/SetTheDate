using Repository;
using SetTheDate.Libraries.Dtos;

namespace SetTheDate.Libraries.Repositories
{
    public class UserRepository : Repository<User, int>
    {
        public UserRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
