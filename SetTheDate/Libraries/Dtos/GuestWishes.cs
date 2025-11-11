using Repository;

namespace SetTheDate.Libraries.Dtos
{
    public class GuestWishes : IEntity
    {
        public int Id { get; set; }
        public int WeddingCardInformationId { get; set; }
        public string Name { get; set; }
        public string Wish { get; set; }

    }
}
