using Repository;

namespace SetTheDate.Libraries.Dtos
{
    public class EventAnswer : IEntity
    {
        public int Id { get; set; }
        public string Answer { get; set; }
        public int EventQuestionId { get; set; }

    }
}
