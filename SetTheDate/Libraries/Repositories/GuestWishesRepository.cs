using Microsoft.EntityFrameworkCore;
using Repository;
using SetTheDate.Libraries.Dtos;

namespace SetTheDate.Libraries.Repositories
{
    public class GuestWishesRepository : Repository<GuestWishes, int>
    {
        public GuestWishesRepository(DbContext context) : base(context)
        {
        }
    }
}
