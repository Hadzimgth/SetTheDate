using Repository;
using SetTheDate.Libraries.Dtos;

namespace SetTheDate.Libraries.Repositories
{
    public class WeddingCardInformationRepository : Repository<WeddingCardInformation, int>
    {
        public WeddingCardInformationRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
