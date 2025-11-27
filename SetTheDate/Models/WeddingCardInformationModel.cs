namespace SetTheDate.Models
{
    public class WeddingCardInformationModel
    {
        public WeddingCardInformationModel()
        {
            ContactInformations = new List<ContactInformationModel>();
            EventImages = new List<EventImageAttachmentModel>();
            GuestWishes = new List<GuestWishesModel>();
        }

        public int Id { get; set; }
        public int UserEventId { get; set; }
        public string GroomName { get; set; }
        public string BrideName { get; set; }
        public string GroomFatherName { get; set; }
        public string GroomMotherName { get; set; }
        public string BrideFatherName { get; set; }
        public string BrideMotherName { get; set; }
        public string Wishes { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string Postcode { get; set; }
        public string State { get; set; }
        public int WeddingCardType { get; set; }
        public int EventImageAttachmentId { get; set; }
        public List<ContactInformationModel> ContactInformations { get; set; }
        public List<EventImageAttachmentModel> EventImages { get; set; }
        public List<GuestWishesModel> GuestWishes { get; set; }
        public string LocationName { get; set; }
        public DateTime TimeFrom { get; set; }
        public DateTime TimeTo { get; set; }

    }
}
