namespace SetTheDate.Models
{
    public class EventSummaryModel
    {
        public int EventId { get; set; }
        public string EventName { get; set; }
        public int CompleteCount { get; set; }
        public int IncompleteCount { get; set; }
        public int NoResponseCount { get; set; }
        public List<GuestResponseDetailModel> GuestResponses { get; set; } = new List<GuestResponseDetailModel>();
    }

    public class GuestResponseDetailModel
    {
        public int GuestId { get; set; }
        public string GuestName { get; set; }
        public string MobileNumber { get; set; }
        public List<QuestionAnswerModel> QuestionAnswers { get; set; } = new List<QuestionAnswerModel>();
        public string ResponseStatus { get; set; } // "Complete", "Incomplete", "No Response"
    }

    public class QuestionAnswerModel
    {
        public int QuestionId { get; set; }
        public string QuestionText { get; set; }
        public string AnswerText { get; set; }
        public int AnswerKeyword { get; set; }
    }
}

