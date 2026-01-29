using AutoMapper;
using SetTheDate.Libraries.Dtos;
using SetTheDate.Libraries.Services;
using SetTheDate.Models;

namespace SetTheDate.ModelFactories
{
    public class SettingModelFactory
    {
        private readonly SettingService _settingService;
        private readonly IMapper _mapper;

        public SettingModelFactory(SettingService settingService, IMapper mapper)
        {
            _settingService = settingService;
            _mapper = mapper;
        }

        public async Task<List<SettingModel>> GetAllSettingsAsync()
        {
            var entities = await _settingService.GetSettings();
            return _mapper.Map<List<SettingModel>>(entities);
        }

        public async Task<SettingModel> InsertSettingAsync(SettingModel settingModel)
        {
            var entity = _mapper.Map<Setting>(settingModel);
            var result = await _settingService.InsertSetting(entity);
            return _mapper.Map<SettingModel>(result);
        }

        public async Task<SettingModel> UpdateSettingAsync(SettingModel settingModel)
        {
            var entity = _mapper.Map<Setting>(settingModel);
            var result = await _settingService.UpdateSetting(entity);
            return _mapper.Map<SettingModel>(result);
        }

        public async Task<SettingModel> GetSettingByIdAsync(int id)
        {
            var entity = await _settingService.GetSettingById(id);
            if (entity == null)
                return null;
            return _mapper.Map<SettingModel>(entity);
        }

        public async Task SaveSettingsAsync(List<SettingModel> settings)
        {
            foreach (var settingModel in settings)
            {
                if (settingModel.Id > 0)
                {
                    // Update existing setting
                    await UpdateSettingAsync(settingModel);
                }
                else if (!string.IsNullOrWhiteSpace(settingModel.Name))
                {
                    // Insert new setting (only if name is not empty)
                    await InsertSettingAsync(settingModel);
                }
            }
        }
    }
}

