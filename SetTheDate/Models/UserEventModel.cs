namespace SetTheDate.Models
{
    public class UserEventModel
    {
        public int Id { get; set; }
        public string EventName { get; set; }
        public string EventDescription { get; set; }
        public DateTime EventDate { get; set; }
        public int UserId { get; set; }

        //weddingcard info
        public string GroomName { get; set; }
        public string BrideName { get; set; }
        public string GroomFatherName { get; set; }
        public string GroomMotherName { get; set; }
        public string BrideFatherName { get; set; }
        public string BrideMotherName { get; set; }
        public string Wishes { get; set; }
        public string VenueName { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string Postcode { get; set; }
        public string State { get; set; }
        public int WeddingCardType { get; set; }
        public int EventImageAttachmentId { get; set; }
        public List<ContactInformationModel> ContactInformations { get; set; }
        public List<EventImageAttachmentModel> EventImages { get; set; }
        public bool IsEdit { get; set; }

    }
}
