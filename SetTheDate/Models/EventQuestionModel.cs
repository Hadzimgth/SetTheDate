namespace SetTheDate.Models
{
    public class EventQuestionModel
    {
        public EventQuestionModel()
        {
            EventAnswerModels = new List<EventAnswerModel>();
        }
        public int Id { get; set; }
        public string Question { get; set; }
        public int UserEventId { get; set; }
        public List<EventAnswerModel> EventAnswerModels { get; set; }

    }
}
