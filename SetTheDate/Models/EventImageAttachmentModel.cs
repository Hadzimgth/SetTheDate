namespace SetTheDate.Models
{
    public class EventImageAttachmentModel
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public string Position { get; set; }
        public string FilePath { get; set; }
        public int UserEventId { get; set; }

    }
}
