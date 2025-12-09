namespace SetTheDate.Models
{
    public class EventSurveySetup
    {
        public EventSurveySetup()
        {
            EventQuestionListModel = new List<EventQuestionModel>();
        }
        public int UserEventId { get; set; }
        public List<EventQuestionModel> EventQuestionListModel { get; set; }
        public bool IsEdit { get; set; }

    }
}
