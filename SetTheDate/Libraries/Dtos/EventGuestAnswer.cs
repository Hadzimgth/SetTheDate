using Repository;

namespace SetTheDate.Libraries.Dtos
{
    public class EventGuestAnswer : IEntity
    {
        public int Id { get; set; }
        public int EventGuestId { get; set; }
        public int EventQuestionId { get; set; }
        public int EventAnswerId { get; set; }
        public int UserEventId { get; set; }

    }
}
