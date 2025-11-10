namespace SetTheDate.Models
{
    public class UserModel
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public bool Verified { get; set; }
        public bool IsAdmin { get; set; }

    }
}
