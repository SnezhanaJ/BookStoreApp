using BookStore.Domain;
using BookStore.Domain.Identity;
using BookStore.Repository;
using BookStore.Repository.Implementation;
using BookStore.Repository.Interface;
using BookStore.Service.Implementation;
using BookStore.Service.Interface;
using Microsoft.EntityFrameworkCore;

internal class Program
{
    private static void Main(string[] args) 
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.Configure<StripeSettings>(builder.Configuration.GetSection("StripeSettings"));
        builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("EmailSettings"));


        // Add services to the container.
        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(connectionString));
        builder.Services.AddDatabaseDeveloperPageExceptionFilter();

        builder.Services.AddDefaultIdentity<BookStoreUsers>(options => options.SignIn.RequireConfirmedAccount = true)
            .AddEntityFrameworkStores<ApplicationDbContext>();
        builder.Services.AddControllersWithViews();

        builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        builder.Services.AddScoped(typeof(IUserRepository), typeof(UserRepository));



        builder.Services.AddTransient<IPublisherService, PublisherService>();
        builder.Services.AddTransient<IAuthorService, AuthorService>();
        builder.Services.AddTransient<IBooksService, BooksService>();
        builder.Services.AddTransient<IShoppingCartService, ShoppingCartService>();
        builder.Services.AddTransient<IEmailService, EmailService>();
        builder.Services.Configure<StripeSettings>(builder.Configuration.GetSection("Stripe"));


        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseMigrationsEndPoint();
        }
        else
        {
            app.UseExceptionHandler("/Home/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthorization();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Books}/{action=Index}/{id?}");
        app.MapRazorPages();

        app.Run();
    }
}