namespace SetTheDate.Models
{
    public class EventGuestListModel
    {
        public EventGuestListModel()
        {
            eventGuestList = new List<EventGuestModel>();
        }

        public int UserEventId { get; set; }
        public IFormFile? GuestFile { get; set; }
        public List<EventGuestModel> eventGuestList { get; set; }
        public string GuestExcelTemplate { get; set; }

    }
}
