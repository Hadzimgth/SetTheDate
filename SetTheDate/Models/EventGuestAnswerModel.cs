namespace SetTheDate.Models
{
    public class EventGuestAnswerModel
    {
        public int Id { get; set; }
        public int EventGuestId { get; set; }
        public int EventQuestionId { get; set; }
        public int EventAnswerId { get; set; }

    }
}
