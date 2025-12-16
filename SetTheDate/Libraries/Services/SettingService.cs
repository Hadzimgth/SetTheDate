using Microsoft.EntityFrameworkCore;
using SetTheDate.Libraries.Dtos;
using SetTheDate.Libraries.Repositories;

namespace SetTheDate.Libraries.Services
{
    public class SettingService
    {
        private readonly ApplicationDbContext _context;
        private readonly SettingRepository _settingRepository;

        public SettingService(
            ApplicationDbContext context, SettingRepository settingRepository)
        {
            _context = context;
            _settingRepository = settingRepository;
        }
        public async Task<List<Setting>> GetSettings()
        {
            return await _settingRepository.GetAllAsync();
        }

    }
}
