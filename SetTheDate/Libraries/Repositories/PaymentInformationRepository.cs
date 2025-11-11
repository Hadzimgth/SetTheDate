using Microsoft.EntityFrameworkCore;
using Repository;
using SetTheDate.Libraries.Dtos;

namespace SetTheDate.Libraries.Repositories
{
    public class PaymentInformationRepository : Repository<PaymentInformation, int>
    {
        public PaymentInformationRepository(DbContext context) : base(context)
        {
        }
    }
}
