using Repository;

namespace SetTheDate.Libraries.Dtos
{
    public class UserEvent : IEntity
    {
        public int Id { get; set; }
        public string EventName { get; set; }
        public string EventDescription { get; set; }
        public DateTime EventDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime PurgeDate { get; set; }
        public bool Completed { get; set; }
        public int UserId { get; set; }
        public int PaymentInformationId { get; set; }
        public int EventImageAttachmentId { get; set; }

    }
}
