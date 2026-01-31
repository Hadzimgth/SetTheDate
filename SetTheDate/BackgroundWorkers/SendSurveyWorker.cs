using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading;
using System.Threading.Tasks;
using SetTheDate.Libraries.Services;

namespace SetTheDate.BackgroundWorkers
{
    public class SendSurveyWorker : BackgroundService
    {
        private readonly ILogger<SendSurveyWorker> _logger;
        private readonly IServiceScopeFactory _scopeFactory;

        // Prevent overlapping executions
        private static readonly SemaphoreSlim _semaphore = new(1, 1);

        public SendSurveyWorker(
            ILogger<SendSurveyWorker> logger,
            IServiceScopeFactory scopeFactory)
        {
            _logger = logger;
            _scopeFactory = scopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("SendSurveyWorker started");

            using var timer = new PeriodicTimer(TimeSpan.FromMinutes(2));

            await RunOnceAsync(stoppingToken);

            while (await timer.WaitForNextTickAsync(stoppingToken))
            {
                await RunOnceAsync(stoppingToken);
            }
        }

        private async Task RunOnceAsync(CancellationToken stoppingToken)
        {
            if (!await _semaphore.WaitAsync(0, stoppingToken))
            {
                _logger.LogWarning("SendSurveyWorker previous run still in progress. Skipping this cycle.");
                return;
            }

            try
            {
                using var scope = _scopeFactory.CreateScope();
                var settingService = scope.ServiceProvider.GetRequiredService<SettingService>();

                // Check if sendsurvey setting is enabled
                var settings = await settingService.GetSettings();

                var sendSurveyEnabled = bool.TryParse(settings.FirstOrDefault(x => x.Name == "sendsurvey")?.Value,out var result)? result : false;

                if (!sendSurveyEnabled)
                {
                    _logger.LogInformation("SendSurveyWorker skipped - sendsurvey setting is disabled");
                    return;
                }

                _logger.LogInformation("SendSurveyWorker execution started at {time}", DateTimeOffset.Now);

                var whatsappService = scope.ServiceProvider.GetRequiredService<WhatsAppService>();

                await whatsappService.SendPendingSurveys(stoppingToken);

                _logger.LogInformation("SendSurveyWorker execution completed at {time}", DateTimeOffset.Now);
            }
            catch (OperationCanceledException)
            {
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "SendSurveyWorker execution failed");
            }
            finally
            {
                _semaphore.Release();
            }
        }
    }
}
