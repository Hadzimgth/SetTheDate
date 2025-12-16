using ExcelDataReader;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PhoneNumbers;
using Repository;
using SetTheDate.Libraries.Dtos;
using SetTheDate.ModelFactories;
using SetTheDate.Models;
using System.Text.Json;
using System.Text;
using SetTheDate.Libraries.Services;

namespace SetTheDate.Controllers
{
    public class MessageController: Controller
    {
        public readonly WhatsAppService _whatsAppService;

        public MessageController(EventModelFactory eventModelFactory, WhatsAppService whatsAppService)
        {
            _whatsAppService = whatsAppService;
        }

        [HttpPost]
        public async Task<IActionResult> Receive()
        {
            try
            {
                using var reader = new StreamReader(Request.Body, Encoding.UTF8);
                var rawBody = await reader.ReadToEndAsync();

                if (string.IsNullOrWhiteSpace(rawBody))
                    return Ok();
                
                var payload = JsonSerializer.Deserialize<WebhookPayload>(
                    rawBody,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                if (payload == null)
                    return Ok();
                
                await _whatsAppService.SaveGuestResponse(payload);

                // ACK immediately
                return Ok();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Webhook error: {ex}");

                return Ok();
            }
        }
    }
}
