using Repository;

namespace SetTheDate.Libraries.Dtos
{
    public class EventGuest : IEntity
    {
        public int Id { get; set; }
        public string GuestName { get; set; }
        public string PhoneNumber { get; set; }
        public int UserEventId { get; set; }
        public bool IsValid { get; set; }

    }
}
