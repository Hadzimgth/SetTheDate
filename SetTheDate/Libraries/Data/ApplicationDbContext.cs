using Microsoft.EntityFrameworkCore;
using SetTheDate.Libraries.Dtos;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<ContactInformation> ContactInformation { get; set; }
    public DbSet<EventAnswer> EventAnswer { get; set; }
    public DbSet<EventGuest> EventGuest { get; set; }
    public DbSet<EventGuestAnswer> EventGuestAnswer { get; set; }
    public DbSet<EventImageAttachment> EventImageAttachment { get; set; }
    public DbSet<EventQuestion> EventQuestion { get; set; }
    public DbSet<GuestWishes> GuestWishes { get; set; }
    public DbSet<PaymentInformation> PaymentInformation { get; set; }
    public DbSet<Setting> Setting { get; set; }
    public DbSet<User> User { get; set; }
    public DbSet<UserEvent> UserEvent { get; set; }
    public DbSet<WeddingCardInformation> WeddingCardInformation { get; set; }
}
