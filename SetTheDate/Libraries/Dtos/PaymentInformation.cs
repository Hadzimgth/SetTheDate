using Repository;

namespace SetTheDate.Libraries.Dtos
{
    public class PaymentInformation : IEntity
    {
        public int Id { get; set; }
        public DateTime PaymentDate { get; set; }
        public decimal Amount { get; set; }
        public string PaymentVia { get; set; }
        public string Status { get; set; }

    }
}
