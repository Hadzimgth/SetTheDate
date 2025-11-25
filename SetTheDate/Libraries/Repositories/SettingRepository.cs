using Repository;
using SetTheDate.Libraries.Dtos;

namespace SetTheDate.Libraries.Repositories
{
    public class SettingRepository : Repository<Setting, int>
    {
        public SettingRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
