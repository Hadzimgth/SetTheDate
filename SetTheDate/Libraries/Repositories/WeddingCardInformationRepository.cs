using Microsoft.EntityFrameworkCore;
using Repository;
using SetTheDate.Libraries.Dtos;

namespace SetTheDate.Libraries.Repositories
{
    public class WeddingCardInformationRepository : Repository<WeddingCardInformation, int>
    {
        public WeddingCardInformationRepository(DbContext context) : base(context)
        {
        }
    }
}
