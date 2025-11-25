using Microsoft.EntityFrameworkCore;
using SetTheDate.Libraries.Dtos;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<ContactInformation> ContactInformations { get; set; }
    public DbSet<EventAnswer> EventAnswers { get; set; }
    public DbSet<EventGuest> EventGuests { get; set; }
    public DbSet<EventGuestAnswer> EventGuestAnswers { get; set; }
    public DbSet<EventImageAttachment> EventImageAttachments { get; set; }
    public DbSet<EventQuestion> EventQuestions { get; set; }
    public DbSet<GuestWishes> GuestWishes { get; set; }
    public DbSet<PaymentInformation> PaymentInformations { get; set; }
    public DbSet<Setting> Settings { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<UserEvent> UserEvents { get; set; }
    public DbSet<WeddingCardInformation> WeddingCardInformations { get; set; }
}
