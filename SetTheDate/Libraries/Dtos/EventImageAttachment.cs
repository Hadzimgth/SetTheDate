using Repository;

namespace SetTheDate.Libraries.Dtos
{
    public class EventImageAttachment : IEntity
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public int UserEventId { get; set; }

    }
}
