using Microsoft.EntityFrameworkCore;
using SetTheDate.Libraries.Dtos;
using SetTheDate.Libraries.Repositories;
using SetTheDate.Models;

namespace SetTheDate.Libraries.Services
{
    public class WhatsAppService
    {
        public readonly EventService _eventService;
        public readonly GuestService _guestService;
        public readonly WasenderClient _wasenderClient;

        public WhatsAppService(EventService eventService, GuestService guestService, WasenderClient wasenderClient)
        {
            _eventService = eventService;
            _guestService = guestService;
            _wasenderClient = wasenderClient;
        }

        public async Task SendPendingSurveys(CancellationToken token)
        {
            var eventList = (await _eventService.GetAllEventListAsync())
                .Where(x => x.Status == "Ongoing" && x.EndDate > DateTime.Now)
                .ToList();

            foreach (var eventModel in eventList)
            {
                token.ThrowIfCancellationRequested();

                var eventId = eventModel.Id;

                var eventSurveyList = await _eventService.GetEventQuestionListByIdAsync(eventId);
                if (!eventSurveyList.Any())
                    continue;

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
                        nextSequence = 1;
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
                        continue;

                    var eventSurvey = eventSurveyList
                        .FirstOrDefault(x => x.Sequence == nextSequence);

                    if (eventSurvey == null)
                        continue;

                    var eventGuestAnswer = new EventGuestAnswer
                    {
                        EventGuestId = guest.Id,
                        EventQuestionId = eventSurvey.Id,
                        EventAnswerId = 0,
                        UserEventId = eventId
                    };

                    var inserted = await _guestService.TryInsertGuestAnswer(eventGuestAnswer);
                    if (!inserted)
                        continue;
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

                    await _wasenderClient.SendMessage(
                        guest.PhoneNumber,
                        messageText
                    );
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
