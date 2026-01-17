using FluentMigrator.Runner;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using SetTheDate.BackgroundWorkers;
using SetTheDate.Libraries;
using SetTheDate.Libraries.Repositories;
using SetTheDate.Libraries.Services;
using SetTheDate.ModelFactories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews()
    .AddRazorRuntimeCompilation();

builder.Services.AddAutoMapper(cfg => { }, typeof(ModelMapper).Assembly);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("ConnectionString");

    options.UseMySql(
        connectionString,
        ServerVersion.AutoDetect(connectionString)
    );
});

builder.Services.AddFluentMigratorCore()
    .ConfigureRunner(rb => rb
        .AddMySql5()
        .WithGlobalConnectionString(
            builder.Configuration.GetConnectionString("ConnectionString")
        )
        .ScanIn(typeof(Program).Assembly).For.Migrations()
    )
    .AddLogging(lb => lb.AddFluentMigratorConsole());

// --- Session ---
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

//backgroundworkers
builder.Services.AddHostedService<SendSurveyWorker>();

//client
builder.Services.AddHttpClient<WasenderClient>(client =>
{
    client.BaseAddress = new Uri("https://www.wasenderapi.com/api/send-message");
    client.Timeout = TimeSpan.FromSeconds(30);
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
builder.Services.AddScoped<PaymentService>();
builder.Services.AddScoped<SettingService>();
builder.Services.AddScoped<WhatsAppService>();

// --- Model Factories ---
builder.Services.AddScoped<UserModelFactory>();
builder.Services.AddScoped<GuestModelFactory>();
builder.Services.AddScoped<EventModelFactory>();
builder.Services.AddScoped<AttachmentModelFactory>();

//validator
builder.Services.AddValidatorsFromAssemblyContaining<ContactInformationModelValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<EventAnswerModelValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<EventSurveySetupValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<GuestWishesModelValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<RegisterModelValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<UserEventModelValidator>();

System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var runner = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();
    runner.MigrateUp();
}
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
