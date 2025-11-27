namespace SetTheDate.Models
{
    public class EventAnswerModel
    {
        public int Id { get; set; }
        public string Answer { get; set; }
        public int EventQuestionId { get; set; }
        public int EventId { get; set; }

    }
}
