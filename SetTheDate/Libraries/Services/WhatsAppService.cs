using SetTheDate.Libraries.Dtos;
using SetTheDate.Models;

namespace SetTheDate.Libraries.Services
{
    public class WhatsAppService
    {
        public readonly EventService _eventService;
        public readonly GuestService _guestService;
        public readonly WasenderClient _wasenderClient;
        private readonly IConfiguration _configuration;
        private readonly ILogger<WhatsAppService> _logger;

        public WhatsAppService(EventService eventService, GuestService guestService, WasenderClient wasenderClient, IConfiguration configuration, ILogger<WhatsAppService> logger)
        {
            _eventService = eventService;
            _guestService = guestService;
            _wasenderClient = wasenderClient;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task SendPendingSurveys(CancellationToken token)
        {

            _logger.LogInformation("Finding Event");
            var eventList = (await _eventService.GetAllEventListAsync())
                .Where(x => x.Status == "Ongoing" && x.EndDate > DateTime.Now)
                .ToList();

            _logger.LogInformation("Finding Event");

            foreach (var eventModel in eventList)
            {
                token.ThrowIfCancellationRequested();

                var eventId = eventModel.Id;


                _logger.LogInformation("Finding question");
                var eventSurveyList = await _eventService.GetEventQuestionListByIdAsync(eventId);
                if (!eventSurveyList.Any())
                    continue;


                _logger.LogInformation("Found question");

                var sentData = await _guestService.GetEventGuestAnswerByEventId(eventId);
                var guestList = await _eventService.GetEventGuestListByIEventdAsync(eventId);
                var answers = await _eventService.GetEventAnswerListByEventIdAsync(eventId);

                foreach (var guest in guestList)
                {
                    token.ThrowIfCancellationRequested();

                    var guestSentData = sentData
                        .Where(x => x.EventGuestId == guest.Id)
                        .OrderBy(x => x.Id)
                        .ToList();

                    var firstAnswer = guestSentData
                        .FirstOrDefault();

                    if (firstAnswer != null && firstAnswer.EventAnswerId == 2)
                    {
                        continue;
                    }

                    int nextSequence;

                    if (guestSentData.Count == 0)
                        nextSequence = 0;
                    else if (guestSentData.Count < eventSurveyList.Count)
                    {
                        nextSequence = guestSentData.Count + 1;

                        var eventSurveyCheck = eventSurveyList
                            .FirstOrDefault(x => x.Sequence == guestSentData.Count);
                        var checkAnswer = await _guestService.GetGuestanswerByGuestMobileNumberAndQuestionId(guest.PhoneNumber, eventSurveyCheck.Id);

                        if (checkAnswer != null && checkAnswer.EventAnswerId == 0)
                            continue;
                    }
                    else
                        nextSequence = int.MaxValue; // No more surveys - will result in eventSurvey == null

                    var eventSurvey = eventSurveyList
                        .FirstOrDefault(x => x.Sequence == nextSequence);

                    if (eventSurvey == null)
                    {
                        // No more surveys to send - check if all surveys are completed and send thank you
                        if (guestSentData.Count > 0 && guestSentData.Count == eventSurveyList.Count)
                        {
                            var allAnswersCompleted = guestSentData.All(x => x.EventAnswerId != 0 && x.EventAnswerId != 2);
                            
                            if (allAnswersCompleted)
                            {
                                // Get wedding card for this event
                                var weddingCard = await _eventService.GetWeddingCardByEventIdAsync(eventId);
                                
                                if (weddingCard != null)
                                {
                                    // Get base URL from configuration or use a default
                                    var baseUrl = _configuration["AppSettings:BaseUrl"] ?? _configuration["AppSettings:BaseURL"] ?? "http://localhost:5123";
                                    var weddingCardLink = $"{baseUrl.TrimEnd('/')}/home/weddingcard?weddingCardId={weddingCard.Id}";
                                    
                                    var thankYouMessage = "Thank you for participating in answering the questions. We greatly appreciate in your participation. In the meantime please visit the eWeddingCard for more information and or leave a wedding wish to them!\n\nVisit it here\n" + weddingCardLink;
                                    
                                    await _wasenderClient.SendMessage(
                                        guest.PhoneNumber,
                                        thankYouMessage
                                    );
                                }
                            }
                        }
                        continue;
                    }


                    _logger.LogInformation("Saving basic response");
                    var eventGuestAnswer = new EventGuestAnswer
                    {
                        EventGuestId = guest.Id,
                        EventQuestionId = eventSurvey.Id,
                        EventAnswerId = 0,
                        UserEventId = eventId
                    };

                    var inserted = await _guestService.TryInsertGuestAnswer(eventGuestAnswer);
                    if (!inserted)
                    {
                        _logger.LogInformation("Failed to insert answer");
                        continue;
                    }
                    var messageText = eventSurvey.Question;

                    var answerOptions = answers
                        .Where(x => x.EventQuestionId == eventSurvey.Id)
                        .OrderBy(x => x.Id)
                        .ToList();

                    if (answerOptions.Any())
                    {
                        messageText += "\n" + string.Join(
                            "\n",
                            answerOptions.Select(x => $"{x.Answer}")
                        );
                    }

                    _logger.LogInformation("Sending message");
                    await _wasenderClient.SendMessage(
                        guest.PhoneNumber,
                        messageText
                    );

                    _logger.LogInformation("Sending success");

                    var random = new Random();
                    int delaySeconds = random.Next(10, 61);

                    await Task.Delay(TimeSpan.FromSeconds(delaySeconds));
                }
            }
        }



        public async Task SaveGuestResponse(WebhookPayload webhookPayload)
        {
            try
            {
                var data = webhookPayload?.Data;
                var messageData = data?.Messages;

                if (messageData == null)
                {
                    Console.WriteLine("No message data found.");
                    return;
                }

                var key = messageData.Key;
                if (key == null) return;
                
                string sender = key.CleanedParticipantPn ?? key.CleanedSenderPn ?? key.RemoteJid;

                string messageContent = messageData.MessageBody;


                if (!string.IsNullOrEmpty(messageContent))
                {
                }

                var eventGuestAnswer = await _guestService.GetGuestanswersByGuestMobileNumber(sender);

                var eventId = eventGuestAnswer.UserEventId;

                var answerList = (await _eventService.GetEventAnswerListByEventIdAsync(eventId)).Where(x => x.EventQuestionId == eventGuestAnswer.EventQuestionId);

                var validAnswers = answerList.Where(x => x.AnswerKeyword == int.Parse(messageContent)).FirstOrDefault();

                if (validAnswers == null)
                {
                    var messageText = "Im sorry, I didnt quite catch that. Please answer based on the number of the previous text I had sent.";

                    await _wasenderClient.SendMessage(
                        sender,
                        messageText
                    );

                    return;
                }

                eventGuestAnswer.EventAnswerId = validAnswers.Id;

                await _guestService.UpdateGuestAnswer(eventGuestAnswer);
            }
            catch (Exception error)
            {
                Console.WriteLine($"Error: {error.Message}");
            }
        }
    }
}
