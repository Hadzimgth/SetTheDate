using Microsoft.AspNetCore.Mvc;
using SetTheDate.ModelFactories;
using SetTheDate.Models;

namespace SetTheDate.Controllers
{
    public class SettingController : Controller
    {
        private readonly SettingModelFactory _settingModelFactory;

        public SettingController(SettingModelFactory settingModelFactory)
        {
            _settingModelFactory = settingModelFactory;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var settings = await _settingModelFactory.GetAllSettingsAsync();
                        
            return View(settings);
        }

        [HttpPost]
        public async Task<IActionResult> Save(List<SettingModel> settings)
        {
            if (settings == null || !settings.Any())
            {
                ModelState.AddModelError("", "No settings data provided.");
                settings = await _settingModelFactory.GetAllSettingsAsync();
                settings.Add(new SettingModel { Id = 0, Name = "", Value = "" });
                return View("Index", settings);
            }

            // Filter out empty rows (where both name and value are empty, but keep existing settings even if value is empty)
            var validSettings = settings
                .Where(s => s.Id > 0 || !string.IsNullOrWhiteSpace(s.Name) || !string.IsNullOrWhiteSpace(s.Value))
                .ToList();

            // For existing settings (Id > 0), ensure they have a name (load from database if missing)
            foreach (var setting in validSettings.Where(s => s.Id > 0 && string.IsNullOrWhiteSpace(s.Name)))
            {
                var existingSetting = await _settingModelFactory.GetSettingByIdAsync(setting.Id);
                if (existingSetting != null)
                {
                    setting.Name = existingSetting.Name;
                }
            }

            // Filter out rows where name is still empty (for new settings)
            validSettings = validSettings
                .Where(s => s.Id > 0 || !string.IsNullOrWhiteSpace(s.Name))
                .ToList();

            if (!validSettings.Any())
            {
                ModelState.AddModelError("", "Please provide at least one setting with a name.");
                settings = await _settingModelFactory.GetAllSettingsAsync();
                settings.Add(new SettingModel { Id = 0, Name = "", Value = "" });
                return View("Index", settings);
            }

            // Validate that all new settings have names
            var newSettingsWithoutNames = validSettings.Where(s => s.Id == 0 && string.IsNullOrWhiteSpace(s.Name)).ToList();
            if (newSettingsWithoutNames.Any())
            {
                ModelState.AddModelError("", "All new settings must have a name.");
                settings = await _settingModelFactory.GetAllSettingsAsync();
                settings.Add(new SettingModel { Id = 0, Name = "", Value = "" });
                return View("Index", settings);
            }

            try
            {
                await _settingModelFactory.SaveSettingsAsync(validSettings);
                TempData["SuccessMessage"] = "Settings saved successfully.";
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"An error occurred while saving settings: {ex.Message}");
                settings = await _settingModelFactory.GetAllSettingsAsync();
                settings.Add(new SettingModel { Id = 0, Name = "", Value = "" });
                return View("Index", settings);
            }

            return RedirectToAction("Index");
        }
    }
}

