using Microsoft.EntityFrameworkCore;
using Repository;
using SetTheDate.Libraries.Dtos;

namespace SetTheDate.Libraries.Repositories
{
    public class ContactInformationRepository : Repository<ContactInformation, int>
    {
        public ContactInformationRepository(DbContext context) : base(context)
        {
        }
    }
}
