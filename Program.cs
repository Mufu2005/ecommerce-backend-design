using Microsoft.AspNetCore.Authentication.Cookies;
using ShopHub.Services;

namespace ShopHub
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // 1. Add Controllers with Views
            builder.Services.AddControllersWithViews();

            // 2. Register your Custom MongoDB Service
            builder.Services.AddSingleton<MongoDbService>();

            // 3. Add Cookie Authentication (Simple & Effective)
            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = "/Account/Login";
                    options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
                    // CRITICAL: These settings ensure the cookie works on localhost
                    options.Cookie.HttpOnly = true;
                    options.Cookie.IsEssential = true;
                    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            // ---------------------------------------------------------
            // THE FIX IS HERE: ORDER MATTERS!
            // ---------------------------------------------------------

            // 1. Check WHO they are first
            app.UseAuthentication();

            // 2. Then Check WHAT they can do
            app.UseAuthorization();

            // ---------------------------------------------------------

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}