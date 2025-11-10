namespace SetTheDate.Models
{
    public class PaymentInformationModel
    {
        public int Id { get; set; }
        public DateTime PaymentDate { get; set; }
        public decimal Amount { get; set; }
        public string PaymentVia { get; set; }
        public string Status { get; set; }

    }
}
