using Repository;
using SetTheDate.Libraries.Dtos;

namespace SetTheDate.Libraries.Repositories
{
    public class GuestWishesRepository : Repository<GuestWishes, int>
    {
        public GuestWishesRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
