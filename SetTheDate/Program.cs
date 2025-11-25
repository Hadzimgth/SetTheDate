using Microsoft.EntityFrameworkCore;
using SetTheDate.Libraries;
using SetTheDate.Libraries.Repositories;
using SetTheDate.Libraries.Services;
using SetTheDate.ModelFactories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddAutoMapper(cfg => { }, typeof(ModelMapper).Assembly);

// --- DbContext ---
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseInMemoryDatabase("TempDb"));
//builder.Services.AddDbContext<ApplicationDbContext>(options =>
//    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// --- Session ---
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// --- Repositories ---
builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<ContactInformationRepository>();
builder.Services.AddScoped<EventAnswerRepository>();
builder.Services.AddScoped<EventGuestRepository>();
builder.Services.AddScoped<EventGuestAnswerRepository>();
builder.Services.AddScoped<EventImageAttachmentRepository>();
builder.Services.AddScoped<EventQuestionRepository>();
builder.Services.AddScoped<GuestWishesRepository>();
builder.Services.AddScoped<PaymentInformationRepository>();
builder.Services.AddScoped<SettingRepository>();
builder.Services.AddScoped<UserEventRepository>();
builder.Services.AddScoped<WeddingCardInformationRepository>();

// --- Services ---
builder.Services.AddScoped<AttachmentService>();
builder.Services.AddScoped<EventService>();
builder.Services.AddScoped<GuestService>();
builder.Services.AddScoped<UserService>();

// --- Model Factories ---
builder.Services.AddScoped<UserModelFactory>();
builder.Services.AddScoped<GuestModelFactory>();
builder.Services.AddScoped<EventModelFactory>();
builder.Services.AddScoped<AttachmentModelFactory>();

var app = builder.Build();

// --- Middleware ---
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseSession();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
