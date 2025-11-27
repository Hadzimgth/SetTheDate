using Repository;

namespace SetTheDate.Libraries.Dtos
{
    public class ContactInformation: IEntity
    {
        public int Id { get; set; }
        public int WeddingCardInformationId { get; set; }
        public string Name { get; set; }
        public string FamilyRole { get; set; }
        public string PhoneNumber { get; set; }

    }
}
