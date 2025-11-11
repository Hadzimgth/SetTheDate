using Microsoft.EntityFrameworkCore;
using Repository;
using SetTheDate.Libraries.Dtos;

namespace SetTheDate.Libraries.Repositories
{
    public class SettingRepository : Repository<Setting, int>
    {
        public SettingRepository(DbContext context) : base(context)
        {
        }
    }
}
