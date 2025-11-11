using Repository;

namespace SetTheDate.Libraries.Dtos
{
    public class EventQuestion : IEntity
    {
        public int Id { get; set; }
        public string Question { get; set; }
        public int UserEventId { get; set; }

    }
}
