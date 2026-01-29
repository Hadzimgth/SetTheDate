using Microsoft.EntityFrameworkCore;
using System.Linq;
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

        public async Task<Setting> InsertSetting(Setting setting)
        {
            _settingRepository.Add(setting);
            await _context.SaveChangesAsync();
            return setting;
        }

        public async Task<Setting> UpdateSetting(Setting setting)
        {
            _settingRepository.Update(setting);
            await _context.SaveChangesAsync();
            return setting;
        }

        public async Task<Setting> GetSettingById(int id)
        {
            return await _settingRepository.GetByIdAsync(id);
        }

    }
}
