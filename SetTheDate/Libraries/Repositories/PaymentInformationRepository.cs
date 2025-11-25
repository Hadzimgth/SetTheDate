using Repository;
using SetTheDate.Libraries.Dtos;

namespace SetTheDate.Libraries.Repositories
{
    public class PaymentInformationRepository : Repository<PaymentInformation, int>
    {
        public PaymentInformationRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
