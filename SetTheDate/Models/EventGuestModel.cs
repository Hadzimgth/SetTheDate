namespace SetTheDate.Models
{
    public class EventGuestModel
    {
        public EventGuestModel()
        {
        }

        public int Id { get; set; }
        public string GuestName { get; set; }
        public string PhoneNumber { get; set; }
        public bool IsValid { get; set; }
        public int UserEventId { get; set; }

    }
}
